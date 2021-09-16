package cmd

import (
	"fmt"
	"log"

	"github.com/spf13/cobra"
	"k8s.io/cli-runtime/pkg/genericclioptions"
	"k8s.io/client-go/kubernetes"
	// anomalydetectionv1alpha1 "AnomalyDetectionOperator/apis/anomalydetection/v1alpha1"
)

type TrainerDistribute struct {
	options *TrainerDistributeOptions

	// client *kubernetes.Clientset
}

type TrainerDistributeOptions struct {
	configFlags *genericclioptions.ConfigFlags

	args []string

	isDatasource   bool
	skipValidation bool

	genericclioptions.IOStreams
}

func NewTrainerDistributeOptions(streams genericclioptions.IOStreams) *TrainerDistributeOptions {
	return &TrainerDistributeOptions{
		configFlags: genericclioptions.NewConfigFlags(true),
		IOStreams:   streams,
	}
}

func NewTrainerDistribute(options *TrainerDistributeOptions) (*TrainerDistribute, error) {

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

	return &TrainerDistribute{
		options: options,
		client:  client,
	}, nil
}

func (o *TrainerDistributeOptions) Complete(cmd *cobra.Command, args []string) error {
	o.args = args

	// o.userSpecifiedNamespace, err = cmd.Flags().GetString("namespace")

	return nil
}

func (o *TrainerDistributeOptions) Validate() error {
	if o.skipValidation {
		return nil
	}

	if *o.configFlags.Namespace == "" {
		return fmt.Errorf("namespace is empty. please set one as -n [namespace]")
	}

	if len(o.args) > 0 {
		return fmt.Errorf("expected no arguments and get {%v}", o.args)
	}
	return nil
}

func (t *TrainerDistribute) Run() (err error) {
	return nil
}

func (o *TrainerDistributeOptions) SetupFlags(cmd *cobra.Command) {
	// --datasource
	cmd.Flags().BoolVar(&o.isDatasource, "datasource", false, "if true, it prints Datasources insted of Trainers")

	// --skip-validation -k
	cmd.Flags().BoolVarP(&o.skipValidation, "skip-validation", "k", false, "if true, skips the arguments validation")

	o.configFlags.AddFlags(cmd.Flags())
}

func NewTrainerDistributeCmd(streams genericclioptions.IOStreams) *cobra.Command {
	o := NewTrainerDistributeOptions(streams)

	t, err := NewTrainerDistribute(o)

	if err != nil {
		log.Fatalf("Failed to obtain a kubernetes client (%s)", err)
	}

	cmd := &cobra.Command{
		Use:   "distribute [FLAGS]",
		Short: "Redistribute the trainer jobs to spread them as much as possible.",
		// Example:      fmt.Sprintf(namespaceExample, "kubectl"),
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
