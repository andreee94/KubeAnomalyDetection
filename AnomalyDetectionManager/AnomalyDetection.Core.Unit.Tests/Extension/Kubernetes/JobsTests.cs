using System.Collections.Generic;
using AnomalyDetection.Core.Extension.Kubernetes;
using FluentAssertions;
using k8s.Models;
using Xunit;

namespace AnomalyDetection.Core.Unit.Tests.Extension.Kubernetes
{
    public class JobsExTests
    {
        [Fact]
        public void ToManualJob_FromCronJob_ShouldReturnValidJob(){
            //Given
            var cronJob = CreateCronJob();

            //When
            var manualJob = cronJob.ToManualJob();

            //Then
            manualJob.Kind.Should().BeSameAs(V1Job.KubeKind);
            manualJob.ApiVersion.Should().BeSameAs($"{V1Job.KubeGroup}/{V1Job.KubeApiVersion}");
            manualJob.Spec.Should().BeEquivalentTo(cronJob.Spec.JobTemplate.Spec);
            manualJob.Metadata.Labels.Should().Equal(cronJob.Metadata.Labels);
        }

        private static V1CronJob CreateCronJob()
        {
            return new V1CronJob
            {
                ApiVersion = $"{V1CronJob.KubeGroup}/{V1CronJob.KubeApiVersion}",
                Kind = V1CronJob.KubeKind,
                Status = new V1CronJobStatus(),
                Metadata = new()
                {
                    Name = "ManualJobName",
                    Labels = new Dictionary<string, string>()
                    {
                        { "MetricName", "metricName" }
                    }
                },
                Spec = new V1CronJobSpec()
                {
                    Schedule = "@daily",
                    JobTemplate = new()
                    {
                        Spec = new()
                        {
                            Template = new()
                            {
                                Spec = new()
                                {
                                    Containers = new List<V1Container>() {
                                        new() {
                                            Name = "ContainerName",
                                            Image = "test/image",
                                            ImagePullPolicy = "Always",
                                            Env = new List<V1EnvVar>
                                            {
                                                new() {
                                                    Name = "Query",
                                                    Value = "empty_query"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}