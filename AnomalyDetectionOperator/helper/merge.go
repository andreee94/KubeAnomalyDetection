package helper

import (
	"github.com/imdario/mergo"
	corev1 "k8s.io/api/core/v1"
)

func MergeContainersWithPriority(first *corev1.Container, containers ...corev1.Container) (err error) {
	for _, c := range containers {
		err = mergo.Merge(
			first, c,
			mergo.WithAppendSlice,
			mergo.WithOverrideEmptySlice,
			mergo.WithOverride,
		)
		if err != nil {
			first = nil
			return
		}
	}

	// Some properties must not append but override:
	// - args
	// - commands
	overridingContainer := *first.DeepCopy()
	for _, c := range containers {
		err = mergo.MergeWithOverwrite(&overridingContainer, c)
		if err != nil {
			first = nil
			return
		}
	}

	first.Args = overridingContainer.Args
	first.Command = overridingContainer.Command
	return
}
