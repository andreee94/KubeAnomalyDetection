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

package v1alpha1

import (
	"fmt"
	"strings"

	corev1 "k8s.io/api/core/v1"
	metav1 "k8s.io/apimachinery/pkg/apis/meta/v1"
)

// TrainerSpec defines the desired state of Trainer
// TODO maybe change LocalObjectReference to TypedLocalObjectReference
type TrainerSpec struct {
	// The query to be executed
	Query string `json:"query"`

	// The name of the AnomalyDetectionDatasource resource.
	Datasource corev1.LocalObjectReference `json:"datasource"`

	// The schedule in Cron format, see https://en.wikipedia.org/wiki/Cron.
	Schedule string `json:"schedule"`

	// Override default options for the AnomalyDetectionTrainer container.
	// +optional
	Container corev1.Container `json:"container,omitempty"`
}

// TrainerStatus defines the observed state of Trainer
type TrainerStatus struct {
	// INSERT ADDITIONAL STATUS FIELD - define observed state of cluster
	// Important: Run "make" to regenerate code after modifying this file

	DatasourceUrl string `json:"url,omitempty"`

	Conditions []StatusCondition `json:"conditions,omitempty"`
}

type ConditionStatus string

var (
	ConditionStatusHealthy   ConditionStatus = "Healthy"
	ConditionStatusUnhealthy ConditionStatus = "Unhealthy"
	ConditionStatusUnknown   ConditionStatus = "Unknown"
)

type StatusCondition struct {
	Type   string          `json:"type"`
	Status ConditionStatus `json:"status"`
	// +optional
	LastProbeTime metav1.Time `json:"lastProbeTime,omitempty"`
	// +optional
	LastTransitionTime metav1.Time `json:"lastTransitionTime,omitempty"`
	// +optional
	Reason string `json:"reason,omitempty"`
	// +optional
	Message string `json:"message,omitempty"`
}

//+kubebuilder:object:root=true
//+kubebuilder:subresource:status

// Trainer is the Schema for the trainers API
type Trainer struct {
	metav1.TypeMeta   `json:",inline"`
	metav1.ObjectMeta `json:"metadata,omitempty"`

	Spec   TrainerSpec   `json:"spec,omitempty"`
	Status TrainerStatus `json:"status,omitempty"`
}

func (t *Trainer) ToRow() string {
	return fmt.Sprintf("%s(%s): Status: %s(%s), Query: %s",
		t.Name,
		t.Spec.Datasource.Name,
		t.Status.Conditions,
		t.Spec.Schedule,
		t.Spec.Query)
}

//+kubebuilder:object:root=true

// TrainerList contains a list of Trainer
type TrainerList struct {
	metav1.TypeMeta `json:",inline"`
	metav1.ListMeta `json:"metadata,omitempty"`
	Items           []Trainer `json:"items"`
}

func (l *TrainerList) ToRows() string {
	builder := strings.Builder{}
	for _, t := range l.Items {
		builder.WriteString(t.ToRow())
	}
	return builder.String()
}

func init() {
	SchemeBuilder.Register(&Trainer{}, &TrainerList{})
}
