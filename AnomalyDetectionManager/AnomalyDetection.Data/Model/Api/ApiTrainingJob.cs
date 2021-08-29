using System;

namespace AnomalyDetection.Data.Model.Api
{
    public class ApiTrainingJob : ApiCrudModel
    {
        public ApiMetric Metric { get; set; }

        public DateTime CreationTime { get; set; }

        public string Status { get; set; }
    }
}