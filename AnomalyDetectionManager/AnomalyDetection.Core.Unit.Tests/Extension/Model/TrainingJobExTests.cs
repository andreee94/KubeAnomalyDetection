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
    public class TrainingJobExTests
    {
        [Fact]
        public void ToCronJob_FromTrainingJob_ShouldReturnValidCronJob()
        {
            //Given
            var trainingJob = CreateTrainingJob();
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
            //Then
        }

        private TrainingJob CreateTrainingJob()
        {
            return new Filler<TrainingJob>().Create();
            // Datasource datasource = new()
            // {
            //     Id = 1,
            //     DatasourceType = "Prometheus",
            //     IsAuthenticated = false,
            //     Url = "127.0.0.1:9090/api/query"
            // };

            // Metric metric = new()
            // {
            //     Id = 1,
            //     Name = "Metric1",
            //     Query = "prometheus_metric1_total",
            //     Datasource = datasource,
            //     TrainingSchedule = "@daily"
            // };

            // return new TrainingJob()
            // {
            //     Id = 1,
            //     Metric = metric,
            //     ExecutionTime = DateTime.Now,
            //     Status = "Done"
            // };
        }
    }
}