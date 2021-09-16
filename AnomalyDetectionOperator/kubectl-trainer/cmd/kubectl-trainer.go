/*
Copyright 2018 The Kubernetes Authors.

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

package main

import (
	"kubectl-trainer/pkg/cmd"
	"log"
	"os"

	"github.com/spf13/cobra"
	"github.com/spf13/pflag"
	"k8s.io/cli-runtime/pkg/genericclioptions"
)

var rootCmd = &cobra.Command{
	Use:          "trainer",
	Long:         "CLI to manage anomalydetection.andreee94.ml/Trainer resources",
	SilenceUsage: true,
}

func main() {
	flags := pflag.NewFlagSet("kubectl-trainer", pflag.ExitOnError)
	pflag.CommandLine = flags

	// root_ns := cmd.NewCmdNamespace(genericclioptions.IOStreams{In: os.Stdin, Out: os.Stdout, ErrOut: os.Stderr})

	// err := root_ns.Execute()
	// if err != nil {
	// 	log.Printf("Executed ns error: {%v}", err)
	// 	// os.Exit(1)
	// }

	// root := cmd.NewCmdTrainer(genericclioptions.IOStreams{In: os.Stdin, Out: os.Stdout, ErrOut: os.Stderr})

	// err := root.Execute()
	// if err != nil {
	// 	log.Printf("Executed trainer error: {%v}", err)
	// 	os.Exit(1)
	// }
	// log.Printf("Executed trainer")

	streams := genericclioptions.IOStreams{In: os.Stdin, Out: os.Stdout, ErrOut: os.Stderr}

	err := NewRootCmd(streams).Execute()
	if err != nil {
		log.Printf("Command failed (%s)", err)
	}
}

// NewCmdMinIO creates a new root command for kubectl-minio
func NewRootCmd(streams genericclioptions.IOStreams) *cobra.Command {
	rootCmd.AddCommand(cmd.NewTrainerCreateCmd(streams))
	rootCmd.AddCommand(cmd.NewTrainerGetCmd(streams))
	rootCmd.AddCommand(cmd.NewTrainerDistributeCmd(streams))
	// rootCmd.AddCommand(cmd.NewCmdTrainer(streams))
	return rootCmd
}
