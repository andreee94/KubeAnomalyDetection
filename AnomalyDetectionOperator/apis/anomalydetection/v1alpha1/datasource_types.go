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

	v1 "k8s.io/api/core/v1"
	metav1 "k8s.io/apimachinery/pkg/apis/meta/v1"
)

type DatasourceType string

const (
	DatasourceTypePrometheus DatasourceType = "Prometheus"
)

// DatasourceSpec defines the desired state of Datasource
type DatasourceSpec struct {
	// The Type of the datasource. Valid values are:
	// - Prometheus
	DatasourceType DatasourceType `json:"datasourceType"`

	// The url of the datasource
	// Ex. http://127.0.0.1:9090
	Url string `json:"url"`

	// If the datasource requires a basic authentication with a username and a password.
	IsAuthenticated bool `json:"isAuthenticated"`

	// Autentication Username.
	// +optional
	UsernameSecretRef v1.SecretKeySelector `json:"usernameSecretRef,omitempty"`

	// Autentication Username.
	// +optional
	PasswordSecretRef v1.SecretKeySelector `json:"passwordSecretRef,omitempty"`
}

// DatasourceStatus defines the observed state of Datasource
type DatasourceStatus struct {
	// INSERT ADDITIONAL STATUS FIELD - define observed state of cluster
	// Important: Run "make" to regenerate code after modifying this file
}

//+kubebuilder:object:root=true
//+kubebuilder:subresource:status

// Datasource is the Schema for the datasources API
type Datasource struct {
	metav1.TypeMeta   `json:",inline"`
	metav1.ObjectMeta `json:"metadata,omitempty"`

	Spec   DatasourceSpec   `json:"spec,omitempty"`
	Status DatasourceStatus `json:"status,omitempty"`
}

func (d *Datasource) ToRow() string {
	return fmt.Sprintf("%s(%s): %s, Authenticated: %t",
		d.Name,
		d.Spec.DatasourceType,
		d.Spec.Url,
		d.Spec.IsAuthenticated)
}

//+kubebuilder:object:root=true

// DatasourceList contains a list of Datasource
type DatasourceList struct {
	metav1.TypeMeta `json:",inline"`
	metav1.ListMeta `json:"metadata,omitempty"`
	Items           []Datasource `json:"items"`
}

func (l *DatasourceList) ToRows() string {
	builder := strings.Builder{}
	for _, d := range l.Items {
		builder.WriteString(d.ToRow())
	}
	return builder.String()
}

func init() {
	SchemeBuilder.Register(&Datasource{}, &DatasourceList{})
}
