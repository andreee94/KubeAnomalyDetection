using System;
using System.Linq;
using FluentAssertions;
using k8s.Models;
using Xunit;
using Tynamix.ObjectFiller;
using AnomalyDetection.Core.Extension.Model.Api;
using System.Collections.Generic;
using AnomalyDetection.Data.Model.Api;

namespace AnomalyDetection.Core.Unit.Tests.Extension.Model.Api
{
    public partial class ApiTrainingJobExTests
    {
        [Fact]
        public void ToCronJob_FromTrainingJob_ShouldReturnValidCronJob()
        {
            //Given
            var trainingJob = CreateRandomTrainingJob();
            const string image = "test/image";
            const string imagePullPolicy = "Always";
            const string restartPolicy = "Never";
            IList<V1EnvVar> envList = new List<V1EnvVar>() { new() { Name = "envName", Value = "envValue" } };
            IList<string> argList = new List<string>() { "arg1", "arg2" };

            //When
            var cronJob = trainingJob.ToCronJob(image, imagePullPolicy, restartPolicy, envList, argList);

            //Then
            cronJob.Kind.Should().BeSameAs(V1CronJob.KubeKind);
            cronJob.ApiVersion.Should().BeSameAs($"{V1CronJob.KubeGroup}/{V1CronJob.KubeApiVersion}");

            var containerList = cronJob.Spec.JobTemplate.Spec.Template.Spec.Containers;
            containerList.Should().HaveCount(1);
            containerList[0].Image.Should().BeSameAs(image);
            containerList[0].ImagePullPolicy.Should().BeSameAs(imagePullPolicy);
            // containerList[0].Args.Should().BeSameAs(argList); // NOT IMPLEMENTED YET
            containerList[0].Env.Should().BeSameAs(envList);
            containerList[0].Env.Should().Contain(x => x.Name == "QUERY");
            cronJob.Spec.JobTemplate.Spec.Template.Spec.RestartPolicy.Should().BeSameAs(restartPolicy);
        }

        [Fact]
        public void GetCronJobName_WithNonNullTrainingJob_ShouldReturnTheCorrectName()
        {
            //Given
            var trainingJob = CreateRandomTrainingJob();
            trainingJob.Metric.Name = "Metric1_Total_Seconds";
            const string expectedName = "trainingjob-metric1-total-seconds";

            //When
            var actualName = trainingJob.GetCronJobName();

            //Then
            actualName.Should().Be(expectedName);
        }

        [Fact]
        public void GetCronJobName_WithNull_ShouldThrowNullReferenceException()
        {
            //Given
            ApiTrainingJob trainingJob1 = null;
            ApiTrainingJob trainingJob2 = CreateRandomTrainingJob();
            trainingJob2.Metric = null;
            ApiTrainingJob trainingJob3 = CreateRandomTrainingJob();
            trainingJob3.Metric.Name = null;

            //When
            Action action1 = () => trainingJob1.GetCronJobName();
            Action action2 = () => trainingJob2.GetCronJobName();
            Action action3 = () => trainingJob3.GetCronJobName();

            //Then
            action1.Should().Throw<NullReferenceException>();
            action2.Should().Throw<NullReferenceException>();
            action3.Should().Throw<NullReferenceException>();
        }
    }
}