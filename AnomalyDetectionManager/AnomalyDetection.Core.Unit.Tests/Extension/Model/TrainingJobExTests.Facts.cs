using System;
using System.Linq;
using AnomalyDetection.Core.Extension.Model;
using AnomalyDetection.Data.Model;
using FluentAssertions;
using k8s.Models;
using Xunit;
using Tynamix.ObjectFiller;

namespace AnomalyDetection.Core.Unit.Tests.Extension.Model
{
    public partial class TrainingJobExTests
    {
        [Fact]
        public void ToCronJob_FromTrainingJob_ShouldReturnValidCronJob()
        {
            //Given
            var trainingJob = CreateRandomTrainingJob();
            const string image = "test/image";
            const string imagePullPolicy = "Always";
            const string restartPolicy = "Never";

            //When
            var cronJob = trainingJob.ToCronJob(image, imagePullPolicy, restartPolicy);

            //Then
            cronJob.Kind.Should().BeSameAs(V1CronJob.KubeKind);
            cronJob.ApiVersion.Should().BeSameAs($"{V1CronJob.KubeGroup}/{V1CronJob.KubeApiVersion}");

            var containerList = cronJob.Spec.JobTemplate.Spec.Template.Spec.Containers;
            containerList.Should().HaveCount(1);
            containerList[0].Image.Should().BeSameAs(image);
            containerList[0].ImagePullPolicy.Should().BeSameAs(imagePullPolicy);
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
            TrainingJob trainingJob1 = null;
            TrainingJob trainingJob2 = CreateRandomTrainingJob();
            trainingJob2.Metric = null;
            TrainingJob trainingJob3 = CreateRandomTrainingJob();
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