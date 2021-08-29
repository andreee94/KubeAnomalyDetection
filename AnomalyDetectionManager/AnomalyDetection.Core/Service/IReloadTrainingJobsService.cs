using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnomalyDetection.Core.Service;

using AnomalyDetection.Data.Repository;
using Microsoft.Extensions.Logging;

namespace AnomalyDetection.Core.Service
{
    public interface IReloadTrainingJobsService : IDisposable
    {
        Task Run();
    }
}