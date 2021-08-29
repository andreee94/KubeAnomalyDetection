using System;
using System.Collections.Generic;
using System.Threading;
using AnomalyDetection.Core.Extension.Model.Api;
using AnomalyDetection.Core.Service;

using AnomalyDetection.Data.Model.Api;
using AnomalyDetection.Data.Model.Option;
using FluentAssertions;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Tynamix.ObjectFiller;
using Xunit;

/////////////////////////////////////////
// THIS TEST DOES NOT WORK 
// SINCE I WAS NOT ABLE TO 
// MOCK THE IKubernetes INTERFACE
// Actually the method I need to mock is 
// an exstension method of the IKubernetes interface
// https://raw.githubusercontent.com/kubernetes-client/csharp/d66c914c86c4fe59fbe8ce2acf3261dbbb754894/src/KubernetesClient/generated/KubernetesExtensions.cs
////////////////////////////////////////

namespace AnomalyDetection.Core.Unit.Tests.Service
{
    public class TrainingJobServiceTests
    {
        protected readonly Random _random = new();
        protected readonly Mock<IKubernetes> _kubernetesStub = new();
        protected readonly Mock<ILogger<TrainingJobService>> _loggerStub = new();
        protected readonly Mock<IOptions<TrainingJobOptions>> _optionsStub = new();

        // [Fact]
        public void ExistsCronJob_WhenJobExists_ShouldReturnTrue()
        {
            //Given
            V1CronJobList cronJobList = new();
            cronJobList.Items = new List<V1CronJob>() {
                CreareRandomCronJob(),
                CreareRandomCronJob(),
                CreareRandomCronJob()
            };
            // _kubernetesStub.Setup(client => client.ListNamespacedCronJobAsync("namespace", null, null, null, null, null, null, null, null, null, null, CancellationToken.None)).ReturnsAsync(cronJobList);

            _kubernetesStub.Setup(client => client.ListNamespacedCronJobAsync(
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(cronJobList);

            var service = new TrainingJobService(_loggerStub.Object, _kubernetesStub.Object, _optionsStub.Object);

            //When
            var result = service.ExistsCronJob(cronJobList.Items[0]);

            //Then
            result.Should().Be(true);
        }

        private static V1CronJob CreareRandomCronJob()
        {
            var trainingJob = new Filler<ApiTrainingJob>().Create();

            return trainingJob.ToCronJob("image", "Always", "Never", new List<V1EnvVar>(), new List<string>());
        }
    }
}