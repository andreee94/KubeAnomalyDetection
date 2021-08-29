using System.Collections.Generic;

namespace AnomalyDetection.Data.Model.Api
{
    public class ApiDatasource : ApiCrudModel
    {
        public string Name { get; set; }
        public string DatasourceType { get; set; }
        public string Url { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}