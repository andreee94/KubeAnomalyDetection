package helper

import (
	"testing"

	. "github.com/onsi/ginkgo"
	. "github.com/onsi/gomega"
	corev1 "k8s.io/api/core/v1"
)

func TestMerge(t *testing.T) {
	RegisterFailHandler(Fail)
	RunSpecs(t, "Merge Suite")
}

var _ = Describe("Merge two containers", func() {
	It("Should merge with correct priority", func() {
		container1 := corev1.Container{
			Name:            "container1",
			Image:           "image1",
			ImagePullPolicy: corev1.PullAlways,
			Env: []corev1.EnvVar{
				{
					Name:  "env11",
					Value: "value11",
				},
				{
					Name:  "env12",
					Value: "value12",
				},
			},
			Args: []string{"bash", "-c"},
		}

		container2 := corev1.Container{
			Name:            "container2",
			ImagePullPolicy: corev1.PullIfNotPresent,
			Env: []corev1.EnvVar{
				{
					Name:  "env21",
					Value: "value21",
				},
				{
					Name:  "env22",
					Value: "value22",
				},
			},
			Args: []string{"bash", "-c", "sleep"},
		}

		container3 := corev1.Container{
			Name: "container3",
			Env: []corev1.EnvVar{
				{
					Name:  "env31",
					Value: "value31",
				},
			},
		}

		expected := corev1.Container{
			Name:            "container3",
			Image:           "image1",
			ImagePullPolicy: corev1.PullIfNotPresent,
			Env: []corev1.EnvVar{
				{
					Name:  "env22",
					Value: "value22",
				},
				{
					Name:  "env11",
					Value: "value11",
				},
				{
					Name:  "env12",
					Value: "value12",
				},
				{
					Name:  "env21",
					Value: "value21",
				},
				{
					Name:  "env31",
					Value: "value31",
				},
			},
			Args: []string{"bash", "-c", "sleep"},
		}

		err := MergeContainersWithPriority(&container1, container2, container3)
		var result = container1

		Expect(err).To(BeNil())
		Expect(result.Name).To(BeEquivalentTo(expected.Name))
		Expect(result.Image).To(BeEquivalentTo(expected.Image))
		Expect(result.ImagePullPolicy).To(BeEquivalentTo(expected.ImagePullPolicy))
		Expect(result.Args).To(BeEquivalentTo(expected.Args))
		Expect(result.ReadinessProbe).To(BeEquivalentTo(expected.ReadinessProbe))
		Expect(result.Env).To(ContainElements(expected.Env))
		Expect(result.Ports).To(ContainElements(expected.Ports))

		// Expect(books).NotTo(ContainElement(book))
	})
})
