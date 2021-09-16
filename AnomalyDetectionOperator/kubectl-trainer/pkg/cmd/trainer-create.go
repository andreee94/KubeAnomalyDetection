package cmd

import (
	"fmt"
	"log"
	"strings"

	"github.com/spf13/cobra"
	corev1 "k8s.io/api/core/v1"
	metav1 "k8s.io/apimachinery/pkg/apis/meta/v1"
	"k8s.io/cli-runtime/pkg/genericclioptions"

	"github.com/ghodss/yaml"

	anomalydetectionv1alpha1 "AnomalyDetectionOperator/apis/anomalydetection/v1alpha1"
	// anomalydetectionv1alpha1 "AnomalyDetectionOperator/apis/anomalydetection/v1alpha1"
)

type TrainerCreate struct {
	options *TrainerCreateOptions

	// client *kubernetes.Clientset
}

type TrainerCreateOptions struct {
	configFlags *genericclioptions.ConfigFlags

	args []string

	isDatasource   bool
	skipValidation bool

	genericclioptions.IOStreams
}

func NewTrainerCreateOptions(streams genericclioptions.IOStreams) *TrainerCreateOptions {
	return &TrainerCreateOptions{
		configFlags: genericclioptions.NewConfigFlags(true),
		IOStreams:   streams,
	}
}

func NewTrainerCreate(options *TrainerCreateOptions) (*TrainerCreate, error) {

	// var client *kubernetes.Clientset
	// clientConfig := options.configFlags.ToRawKubeConfigLoader()
	// restConfig, err := clientConfig.ClientConfig()

	// if err != nil {
	// 	return nil, err
	// }

	// client, err = kubernetes.NewForConfig(restConfig)

	// if err != nil {
	// 	return nil, err
	// }

	return &TrainerCreate{
		options: options,
		// client:  client,
	}, nil
}

func (o *TrainerCreateOptions) Complete(cmd *cobra.Command, args []string) error {
	o.args = args

	// o.userSpecifiedNamespace, err = cmd.Flags().GetString("namespace")

	return nil
}

func (o *TrainerCreateOptions) Validate() error {
	if o.skipValidation {
		return nil
	}

	if len(o.args) > 0 {
		return fmt.Errorf("expected no arguments and get {%v}", o.args)
	}
	return nil
}

func (t *TrainerCreate) Run() (err error) {

	var namespace string

	if t.options.configFlags.Namespace == nil || *t.options.configFlags.Namespace == "" {
		namespace = "default"
	} else {
		namespace = *t.options.configFlags.Namespace
	}

	secret := GetExampleSecret(namespace)
	datasource := GetExampleDatasource(namespace)
	trainer := GetExampleTrainer(namespace)

	var yaml string

	if t.options.isDatasource {
		yaml, err = GetResourcesYaml(secret, datasource)
	} else {
		yaml, err = GetResourcesYaml(secret, datasource, trainer)
	}

	fmt.Println(yaml)
	log.Println(err)

	return nil
}

func (o *TrainerCreateOptions) SetupFlags(cmd *cobra.Command) {
	// --datasource
	cmd.Flags().BoolVar(&o.isDatasource, "datasource", false, "if true, it prints Datasources insted of Trainers")

	// --skip-validation -k
	cmd.Flags().BoolVarP(&o.skipValidation, "skip-validation", "k", false, "if true, skips the arguments validation")

	o.configFlags.AddFlags(cmd.Flags())
}

func NewTrainerCreateCmd(streams genericclioptions.IOStreams) *cobra.Command {
	o := NewTrainerCreateOptions(streams)

	t, err := NewTrainerCreate(o)

	if err != nil {
		log.Fatalf("Failed to obtain a kubernetes client (%s)", err)
	}

	cmd := &cobra.Command{
		Use:          "create [--datasource] [FLAGS]",
		Short:        "Get an example yaml file for a Trainer or a Datasource in the namespace",
		Example:      fmt.Sprintf(namespaceExample, "kubectl"),
		SilenceUsage: true,
		RunE: func(c *cobra.Command, args []string) error {
			if err := o.Complete(c, args); err != nil {
				return err
			}
			if err := o.Validate(); err != nil {
				return err
			}
			if err := t.Run(); err != nil {
				return err
			}

			return nil
		},
	}

	o.SetupFlags(cmd)

	return cmd
}

func GetResourcesYaml(objects ...interface{}) (string, error) {
	builder := strings.Builder{}
	for _, o := range objects {
		y, err := yaml.Marshal(o)
		if err != nil {
			return "", err
		}
		builder.WriteString("---\n")
		builder.WriteString(string(y))
	}
	return builder.String(), nil
}

func GetExampleSecret(namespace string) interface{} {
	secret := corev1.Secret{
		TypeMeta: metav1.TypeMeta{
			Kind:       "Secret",
			APIVersion: "v1",
		},
		ObjectMeta: metav1.ObjectMeta{
			Name:      "example-prometheus-secret",
			Namespace: namespace,
		},
		Type: corev1.SecretTypeBasicAuth,
		StringData: map[string]string{
			"username": "prometheus-username",
			"password": "prometheus-password",
		},
	}
	return secret
}

func GetExampleDatasource(namespace string) interface{} {

	datasource := anomalydetectionv1alpha1.Datasource{
		TypeMeta: metav1.TypeMeta{
			Kind:       "Datasource",
			APIVersion: "anomalydetection.andreee94.ml/v1alpha1",
		},
		ObjectMeta: metav1.ObjectMeta{
			Name:      "example-datasource",
			Namespace: namespace,
		},
		Spec: anomalydetectionv1alpha1.DatasourceSpec{
			DatasourceType:  anomalydetectionv1alpha1.DatasourceTypePrometheus,
			Url:             "http://127.0.0.1:9090",
			IsAuthenticated: true,
			UsernameSecretRef: corev1.SecretKeySelector{
				LocalObjectReference: corev1.LocalObjectReference{
					Name: "example-prometheus-secret",
				},
				Key: "username",
			},
			PasswordSecretRef: corev1.SecretKeySelector{
				LocalObjectReference: corev1.LocalObjectReference{
					Name: "example-prometheus-secret",
				},
				Key: "password",
			},
		},
	}
	return datasource
}

func GetExampleTrainer(namespace string) interface{} {

	trainer := anomalydetectionv1alpha1.Trainer{
		TypeMeta: metav1.TypeMeta{
			Kind:       "Trainer",
			APIVersion: "anomalydetection.andreee94.ml/v1alpha1",
		},
		ObjectMeta: metav1.ObjectMeta{
			Name:      "example-trainer",
			Namespace: namespace,
		},
		Spec: anomalydetectionv1alpha1.TrainerSpec{
			Query:    "up",
			Schedule: "0 */2 * * *",
			Datasource: corev1.LocalObjectReference{
				Name: "example-datasource",
			},
			Container: corev1.Container{
				Name:  "training-job-container",
				Image: "andreee94/anomaly-detection-trainer:latest",
				Env: []corev1.EnvVar{
					{Name: "ExampleEnv", Value: "Value"},
				},
			},
		},
	}
	return trainer
}
