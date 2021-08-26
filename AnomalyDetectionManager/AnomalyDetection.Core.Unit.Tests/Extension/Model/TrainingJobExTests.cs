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
        private static TrainingJob CreateRandomTrainingJob()
        {
            return new Filler<TrainingJob>().Create();
        }
    }
}