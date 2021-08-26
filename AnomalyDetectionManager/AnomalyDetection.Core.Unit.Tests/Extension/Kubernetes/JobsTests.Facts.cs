using System.Collections.Generic;
using AnomalyDetection.Core.Extension.Kubernetes;
using FluentAssertions;
using k8s.Models;
using Xunit;

namespace AnomalyDetection.Core.Unit.Tests.Extension.Kubernetes
{
    public partial class JobsExTests
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
    }
}