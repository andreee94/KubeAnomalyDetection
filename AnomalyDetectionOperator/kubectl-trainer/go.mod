// This is a generated file. Do not edit directly.

module kubectl-trainer

go 1.16

require (
	AnomalyDetectionOperator v0.0.0-00010101000000-000000000000
	github.com/ghodss/yaml v1.0.0
	github.com/spf13/cobra v1.2.1
	github.com/spf13/pflag v1.0.5
	k8s.io/api v0.22.1
	k8s.io/apimachinery v0.22.1
	k8s.io/cli-runtime v0.22.1
	k8s.io/client-go v0.22.1
)

replace (
	k8s.io/api => k8s.io/api v0.0.0-20210909233056-897e446fab01
	k8s.io/apimachinery => k8s.io/apimachinery v0.0.0-20210909232852-2694a9d8c2a6
	k8s.io/cli-runtime => k8s.io/cli-runtime v0.0.0-20210909235447-7c51b598b842
	k8s.io/client-go => k8s.io/client-go v0.0.0-20210909233348-92773dec0039
)

replace AnomalyDetectionOperator => ../../AnomalyDetectionOperator
