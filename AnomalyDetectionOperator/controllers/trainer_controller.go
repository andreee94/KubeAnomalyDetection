/*
Copyright 2021.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

package controllers

import (
	"context"

	"github.com/go-logr/logr"
	batchv1 "k8s.io/api/batch/v1"
	corev1 "k8s.io/api/core/v1"

	// core "k8s.io/api/core/v1"

	"k8s.io/apimachinery/pkg/api/errors"
	metav1 "k8s.io/apimachinery/pkg/apis/meta/v1"
	"k8s.io/apimachinery/pkg/runtime"
	"k8s.io/apimachinery/pkg/types"
	ctrl "sigs.k8s.io/controller-runtime"
	"sigs.k8s.io/controller-runtime/pkg/client"
	"sigs.k8s.io/controller-runtime/pkg/handler"
	"sigs.k8s.io/controller-runtime/pkg/log"
	"sigs.k8s.io/controller-runtime/pkg/reconcile"
	"sigs.k8s.io/controller-runtime/pkg/source"

	anomalydetectionv1alpha1 "AnomalyDetectionOperator/api/v1alpha1"
)

func ignoreNotFound(err error) error {
	if errors.IsNotFound(err) {
		return nil
	}
	return err
}

// TrainerReconciler reconciles a Trainer object
type TrainerReconciler struct {
	client.Client
	Log    logr.Logger
	Scheme *runtime.Scheme
}

//+kubebuilder:rbac:groups=anomalydetection.andreee94.ml,resources=trainers,verbs=get;list;watch;create;update;patch;delete
//+kubebuilder:rbac:groups=anomalydetection.andreee94.ml,resources=trainers/status,verbs=get;update;patch
//+kubebuilder:rbac:groups=anomalydetection.andreee94.ml,resources=trainers/finalizers,verbs=update

// Reconcile is part of the main kubernetes reconciliation loop which aims to
// move the current state of the cluster closer to the desired state.
// TODO(user): Modify the Reconcile function to compare the state specified by
// the Trainer object against the actual cluster state, and then
// perform operations to make the cluster state reflect the state specified by
// the user.
//
// For more details, check Reconcile and its Result here:
// - https://pkg.go.dev/sigs.k8s.io/controller-runtime@v0.8.3/pkg/reconcile
func (r *TrainerReconciler) Reconcile(ctx context.Context, req ctrl.Request) (ctrl.Result, error) {
	var err error
	logger := log.FromContext(ctx)

	var app anomalydetectionv1alpha1.Trainer
	if err := r.Get(ctx, req.NamespacedName, &app); err != nil {
		// it might be not found if this is a delete request
		if ignoreNotFound(err) == nil {
			return ctrl.Result{}, nil
		}
		logger.Error(err, "unable to fetch trainers")
		return ctrl.Result{}, err
	}

	cronJob := &batchv1.CronJob{
		ObjectMeta: metav1.ObjectMeta{
			Name:      req.Name,
			Namespace: req.Namespace,
		},
		// Spec: batchv1.CronJobSpec{
		// Schedule: "",
		// JobTemplate: batchv1.JobTemplateSpec{
		// ObjectMeta: metav1.ObjectMeta{
		// 	Name:      req.Name,
		// 	Namespace: req.Namespace,
		// },
		// Spec: batchv1.JobSpec{
		// 	Template: corev1.PodTemplateSpec{
		// 		Spec: corev1.PodSpec{
		// 			Containers: []corev1.Container{
		// 				{
		// 					Name:            "training-job-container",
		// 					Image:           "alpine",
		// 					ImagePullPolicy: corev1.PullIfNotPresent,
		// 					Command:         []string{"sh", "-c"},
		// 					Args:            []string{"sleep 10 && echo ${Query}"},
		// 					Env: []corev1.EnvVar{
		// 						{
		// 							Name:  "Query",
		// 							Value: app.Spec.Query,
		// 						},
		// 					},
		// 				},
		// 			},
		// RestartPolicy: corev1.RestartPolicyNever,
		// 			// ImagePullSecrets: []corev1.LocalObjectReference{},
		// 		},
		// 	},
		// },
		// 	},
		// },
	}

	var datasource anomalydetectionv1alpha1.Datasource
	err = r.Get(ctx, types.NamespacedName{Name: app.Spec.Datasource.Name, Namespace: app.Namespace}, &datasource)
	if err != nil {
		logger.Error(err, "unable to fetch corresponding datasource", "datasource", app.Spec.Datasource.Name)
		return ctrl.Result{}, ignoreNotFound(err)
	}
	logger.Info("using datasource object", "datasourceType", datasource.Spec.DatasourceType, "datasource.name", datasource.Name, "datasourceUrl", datasource.Spec.Url)

	_, err = ctrl.CreateOrUpdate(ctx, r.Client, cronJob, func() error {

		if cronJob.Spec.JobTemplate.ObjectMeta.Labels == nil {
			cronJob.Spec.JobTemplate.ObjectMeta.Labels = map[string]string{}
		}

		cronJob.Spec.JobTemplate.ObjectMeta.Labels["trainer-job"] = req.Name

		// cronJob.Spec.JobTemplate.Spec.Selector = &metav1.LabelSelector{
		// 	MatchLabels: map[string]string{
		// 		"trainer-job": req.Name,
		// 	}}

		// Configure cronjob settings
		cronJob.Spec.Schedule = app.Spec.Schedule
		cronJob.Spec.JobTemplate.Spec.Template.Spec.RestartPolicy = corev1.RestartPolicyNever

		container := getContainerFromName(*cronJob, "training-job-container")

		if container == nil {
			cronJob.Spec.JobTemplate.Spec.Template.Spec.Containers = append(
				cronJob.Spec.JobTemplate.Spec.Template.Spec.Containers,
				corev1.Container{Name: "training-job-container"},
			)
			containersCount := len(cronJob.Spec.JobTemplate.Spec.Template.Spec.Containers)
			container = &cronJob.Spec.JobTemplate.Spec.Template.Spec.Containers[containersCount-1]
		}

		// Configure cronjob container settings
		container.Image = "alpine"
		container.ImagePullPolicy = corev1.PullIfNotPresent
		container.Command = []string{"sh", "-c"}
		container.Args = []string{"sleep 10 && echo ${Query}"}
		setEnv(container, "Query", app.Spec.Query)
		setEnv(container, "DatasourceUrl", datasource.Spec.Url)

		// set the owner so that garbage collection kicks in
		if err := ctrl.SetControllerReference(&app, cronJob, r.Scheme); err != nil {
			return err
		}

		setCondition(&app.Status.Conditions, anomalydetectionv1alpha1.StatusCondition{
			Type:    "DeploymentUpToDate",
			Status:  anomalydetectionv1alpha1.ConditionStatusHealthy,
			Reason:  "EnsuredDeployment",
			Message: "Ensured deployment was up to date",
		})

		return nil
	})

	// CreateOrUpdate fails
	if err != nil {
		logger.Error(err, "unable to ensure deployment is correct")
		setCondition(&app.Status.Conditions, anomalydetectionv1alpha1.StatusCondition{
			Type:    "DeploymentUpToDate",
			Status:  anomalydetectionv1alpha1.ConditionStatusUnhealthy,
			Reason:  "UpdateError",
			Message: "Unable to fetch or update deployment",
		})

		err := r.Status().Update(ctx, &app)
		if err != nil {
			logger.Error(err, "unable to update trainer status")
		}
		return ctrl.Result{}, err
	}

	// Updating the status
	app.Status.DatasourceUrl = datasource.Spec.Url

	err = r.Status().Update(ctx, &app)
	if err != nil {
		logger.Error(err, "unable to update trainer status")
		return ctrl.Result{}, err
	}

	return ctrl.Result{}, nil
}

func getContainerFromName(cronJob batchv1.CronJob, containerName string) *corev1.Container {
	var container *corev1.Container
	// find the right container
	containers := cronJob.Spec.JobTemplate.Spec.Template.Spec.Containers
	for i, iterCont := range containers {
		if iterCont.Name == containerName {
			container = &cronJob.Spec.JobTemplate.Spec.Template.Spec.Containers[i]
			return container
		}
	}
	return nil
}

func setEnv(cont *corev1.Container, key, val string) {
	var envVar *corev1.EnvVar
	for i, iterVar := range cont.Env {
		if iterVar.Name == key {
			envVar = &cont.Env[i] // index to avoid capturing the iteration variable
			break
		}
	}
	if envVar == nil {
		cont.Env = append(cont.Env, corev1.EnvVar{
			Name: key,
		})
		envVar = &cont.Env[len(cont.Env)-1]
	}
	envVar.Value = val
}

func setCondition(conds *[]anomalydetectionv1alpha1.StatusCondition, targetCond anomalydetectionv1alpha1.StatusCondition) {
	var outCond *anomalydetectionv1alpha1.StatusCondition
	for i, cond := range *conds {
		if cond.Type == targetCond.Type {
			outCond = &(*conds)[i]
			break
		}
	}
	if outCond == nil {
		*conds = append(*conds, targetCond)
		outCond = &(*conds)[len(*conds)-1]
		outCond.LastTransitionTime = metav1.Now()
	} else {
		lastState := outCond.Status
		lastTrans := outCond.LastTransitionTime
		*outCond = targetCond
		if outCond.Status != lastState {
			outCond.LastTransitionTime = metav1.Now()
		} else {
			outCond.LastTransitionTime = lastTrans
		}
	}

	outCond.LastProbeTime = metav1.Now()
}

// SetupWithManager sets up the controller with the Manager.
func (r *TrainerReconciler) SetupWithManager(mgr ctrl.Manager) error {
	mgr.GetFieldIndexer().IndexField(context.Background(), &anomalydetectionv1alpha1.Trainer{}, ".spec.datasource.name", func(obj client.Object) []string {
		datasourceName := obj.(*anomalydetectionv1alpha1.Trainer).Spec.Datasource.Name
		if datasourceName == "" {
			return nil
		}
		return []string{datasourceName}
	})

	return ctrl.NewControllerManagedBy(mgr).
		For(&anomalydetectionv1alpha1.Trainer{}).
		// Owns(&apps.Deployment{}).
		Owns(&batchv1.CronJob{}).
		// Owns(&core.Service{}).
		Watches(&source.Kind{Type: &anomalydetectionv1alpha1.Datasource{}},
			handler.EnqueueRequestsFromMapFunc(func(obj client.Object) []reconcile.Request {
				var trainers anomalydetectionv1alpha1.TrainerList

				err := r.List(context.Background(),
					&trainers,
					client.InNamespace(obj.GetNamespace()),
					client.MatchingFields(map[string]string{".spec.datasource.name": obj.GetName()}),
				)
				if err != nil {
					r.Log.Info("unable to get trainer for datasource", "datasource", obj)
					return nil
				}
				res := make([]reconcile.Request, len(trainers.Items))
				for i, trainer := range trainers.Items {
					res[i].Name = trainer.Name
					res[i].Namespace = trainer.Namespace
				}
				return res
			}),
		).
		Complete(r)
}
