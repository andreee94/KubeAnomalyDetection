package cmd

import (
	"context"
	"fmt"
	"log"

	"github.com/spf13/cobra"
	"k8s.io/apimachinery/pkg/runtime"
	"k8s.io/cli-runtime/pkg/genericclioptions"
	"k8s.io/client-go/kubernetes"

	anomalydetectionv1alpha1 "AnomalyDetectionOperator/apis/anomalydetection/v1alpha1"
	// anomalydetectionv1alpha1 "AnomalyDetectionOperator/apis/anomalydetection/v1alpha1"
)

type TrainerGet struct {
	options *TrainerGetOptions

	client *kubernetes.Clientset
}

type TrainerGetOptions struct {
	configFlags *genericclioptions.ConfigFlags

	args []string

	isDatasource   bool
	skipValidation bool

	genericclioptions.IOStreams
}

func NewTrainerGetOptions(streams genericclioptions.IOStreams) *TrainerGetOptions {
	return &TrainerGetOptions{
		configFlags: genericclioptions.NewConfigFlags(true),
		IOStreams:   streams,
	}
}

func NewTrainerGet(options *TrainerGetOptions) (*TrainerGet, error) {

	var client *kubernetes.Clientset
	clientConfig := options.configFlags.ToRawKubeConfigLoader()
	restConfig, err := clientConfig.ClientConfig()

	if err != nil {
		return nil, err
	}

	client, err = kubernetes.NewForConfig(restConfig)

	if err != nil {
		return nil, err
	}

	return &TrainerGet{
		options: options,
		client:  client,
	}, nil
}

func (o *TrainerGetOptions) Complete(cmd *cobra.Command, args []string) error {
	o.args = args

	// o.userSpecifiedNamespace, err = cmd.Flags().GetString("namespace")

	return nil
}

func (o *TrainerGetOptions) Validate() error {
	if o.skipValidation {
		return nil
	}

	if len(o.args) > 0 {
		return fmt.Errorf("expected no arguments and get {%v}", o.args)
	}
	return nil
}

func (t *TrainerGet) Run() (err error) {

	if t.options.isDatasource {
		datasources := anomalydetectionv1alpha1.DatasourceList{}
		err = t.GetCRD("datasources", nil, &datasources)

		log.Println(datasources.ToRows())
	} else {
		trainers := anomalydetectionv1alpha1.TrainerList{}
		err = t.GetCRD("trainers", nil, &trainers)

		log.Println(trainers.ToRows())
	}

	// namespaces, err := t.client.CoreV1().Namespaces().List(context.Background(), metav1.ListOptions{})

	log.Println(err)

	return nil
}

func (t *TrainerGet) GetCRD(kind string, name *string, output runtime.Object) error {

	request := t.client.
		RESTClient().
		Get().
		AbsPath("/apis/anomalydetection.andreee94.ml/v1alpha1").
		Namespace(*t.options.configFlags.Namespace).
		Resource(kind)

	if name != nil {
		request.Name(*name)
	}

	err := request.
		Do(context.Background()).
		Into(output)

	return err

}

func (o *TrainerGetOptions) SetupFlags(cmd *cobra.Command) {
	// --datasource
	cmd.Flags().BoolVar(&o.isDatasource, "datasource", false, "if true, it prints Datasources insted of Trainers")

	// --skip-validation -k
	cmd.Flags().BoolVarP(&o.skipValidation, "skip-validation", "k", false, "if true, skips the arguments validation")

	o.configFlags.AddFlags(cmd.Flags())
}

func NewTrainerGetCmd(streams genericclioptions.IOStreams) *cobra.Command {
	o := NewTrainerGetOptions(streams)

	t, err := NewTrainerGet(o)

	if err != nil {
		log.Fatalf("Failed to obtain a kubernetes client (%s)", err)
	}

	cmd := &cobra.Command{
		Use:          "get [--datasource] [FLAGS]",
		Short:        "Get the installed Trainer and Datasource resources in the namespace",
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
