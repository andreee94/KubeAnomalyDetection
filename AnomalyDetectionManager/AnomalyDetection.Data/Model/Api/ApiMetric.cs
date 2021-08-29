namespace AnomalyDetection.Data.Model.Api
{
    public class ApiMetric : ApiCrudModel
    {
        public string Name { get; set; }

        public string Query { get; set; }

        public string TrainingSchedule { get; set; }

        public ApiDatasource Datasource { get; set; }
    }
}