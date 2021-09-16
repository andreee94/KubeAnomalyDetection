package cmd

import (
	"encoding/json"
	"fmt"
	"log"

	"github.com/spf13/cobra"
	"k8s.io/cli-runtime/pkg/genericclioptions"
)

type TrainerOptions struct {
	configFlags *genericclioptions.ConfigFlags

	// resultingContext     *api.Context
	// resultingContextName string

	// userSpecifiedCluster   string
	// userSpecifiedContext   string
	// userSpecifiedAuthInfo  string
	// userSpecifiedNamespace string

	// rawConfig      api.Config
	// listNamespaces bool
	args []string

	genericclioptions.IOStreams
}

func NewTrainerOptions(streams genericclioptions.IOStreams) *TrainerOptions {
	return &TrainerOptions{
		configFlags: genericclioptions.NewConfigFlags(true),
		IOStreams:   streams,
	}
}

func (o *TrainerOptions) Complete(cmd *cobra.Command, args []string) error {
	o.args = args

	// o.userSpecifiedNamespace, err = cmd.Flags().GetString("namespace")

	return nil
}

func (o *TrainerOptions) Validate() error {
	// if len(o.args) > 1 {
	// 	return fmt.Errorf("either one or no arguments are allowed")
	// }
	return nil
}

func (o *TrainerOptions) Run() error {

	for i, arg := range o.args {
		log.Printf("index: {%d}, value: {%s}", i, arg)
	}

	log.Printf("configFlags:")
	str, _ := json.MarshalIndent(o.configFlags, "", " ")
	log.Println(str)

	log.Printf("configFlags.Namespace: {%s}", *o.configFlags.Namespace)

	log.Println("Running trainer command.")
	return nil
}

func NewCmdTrainer(streams genericclioptions.IOStreams) *cobra.Command {
	o := NewTrainerOptions(streams)

	cmd := &cobra.Command{
		Use:          "ns [new-namespace] [flags]",
		Short:        "View or set the current namespace",
		Example:      fmt.Sprintf(namespaceExample, "kubectl"),
		SilenceUsage: true,
		RunE: func(c *cobra.Command, args []string) error {
			if err := o.Complete(c, args); err != nil {
				return err
			}
			if err := o.Validate(); err != nil {
				return err
			}
			if err := o.Run(); err != nil {
				return err
			}

			return nil
		},
	}

	// cmd.Flags().BoolVar(&o.listNamespaces, "list", o.listNamespaces, "if true, print the list of all namespaces in the current KUBECONFIG")
	o.configFlags.AddFlags(cmd.Flags())

	return cmd
}
