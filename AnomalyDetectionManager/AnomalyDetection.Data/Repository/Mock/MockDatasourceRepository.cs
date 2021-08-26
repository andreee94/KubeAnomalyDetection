using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model;

namespace AnomalyDetection.Data.Repository.Mock
{
    public class MockDatasourceRepository : IDatasourceRepository
    {
        private readonly IList<Datasource> _datasourceList;

        private readonly Random _random;

        public MockDatasourceRepository()
        {
            _random = new Random();
            _datasourceList = new List<Datasource> ();

            // _datasourceList = new List<Datasource> {
            //     new()
            //     {
            //         Id = 1,
            //         DatasourceType = "Prometheus",
            //         IsAuthenticated = false,
            //         Url = "127.0.0.1:9090/api/query"
            //     }
            // };
        }

        public Task<Datasource?> AddAsync(Datasource datasource)
        {
            datasource.Id = _random.Next(1000);
            _datasourceList.Add(datasource);

            return Task.FromResult((Datasource?)datasource);
        }

        public Task<Datasource?> DeleteByIdAsync(int id)
        {
            Datasource? datasource = _datasourceList.FirstOrDefault(item => item.Id == id);

            if (datasource is not null)
            {
                _datasourceList.Remove(datasource);
                return Task.FromResult((Datasource?)datasource);
            }

            return Task.FromResult((Datasource?)null);
        }

        public Task<Datasource?> EditAsync(int id, Datasource newDatasource)
        {
            Datasource? datasource = _datasourceList.FirstOrDefault(item => item.Id == id);

            if (datasource is not null)
            {
                datasource.DatasourceType = newDatasource.DatasourceType;
                datasource.Url = newDatasource.Url;
                datasource.IsAuthenticated = newDatasource.IsAuthenticated;
                datasource.Username = newDatasource.Username;
                datasource.Password = newDatasource.DatasourceType;
                return Task.FromResult((Datasource?)datasource);
            }
            return Task.FromResult((Datasource?)null);
        }

        public Task<IList<Datasource>> GetAllAsync()
        {
            return Task.FromResult(_datasourceList);
        }

        public Task<Datasource?> GetByIdAsync(int id)
        {
            return Task.FromResult(_datasourceList.FirstOrDefault(item => item.Id == id));
        }
    }
}