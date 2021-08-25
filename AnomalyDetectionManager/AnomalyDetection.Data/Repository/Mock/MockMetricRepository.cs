using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Data.Model;

namespace AnomalyDetection.Data.Repository.Mock
{
    public class MockMetricRepository : IMetricRepository
    {
        private readonly IList<Metric> _metricList;

        private readonly Random _random;

        public MockMetricRepository()
        {
            _random = new Random();

            Datasource prometheus = new()
            {
                Id = 1,
                DatasourceType = "Prometheus",
                IsAuthenticated = false,
                Url = "127.0.0.1:9090/api/query"
            };

            _metricList = new List<Metric> {
                new () {
                Id = 1,
                    Name = "Metric1",
                    Query = "prometheus_metric1_total",
                    Datasource = prometheus
                },
                new () {
                Id = 2,
                    Name = "Metric2",
                    Query = "prometheus_metric2_total",
                    Datasource = prometheus
                },
                new () {
                Id = 3,
                    Name = "Metric2",
                    Query = "prometheus_metric2_total",
                    Datasource = prometheus
                }
            };
        }

        public Task<Metric?> AddAsync(Metric metric)
        {
            metric.Id = _random.Next(1000);
            _metricList.Add(metric);

            return Task.FromResult((Metric?)metric);
        }

        public Task<Metric?> DeleteByIdAsync(int id)
        {
            Metric? metric = _metricList.FirstOrDefault(item => item.Id == id);

            if (metric is not null)
            {
                _metricList.Remove(metric);
                return Task.FromResult((Metric?)metric);
            }

            return Task.FromResult((Metric?)null);
        }

        public Task<Metric?> EditAsync(int id, Metric newMetric)
        {
            Metric? metric = _metricList.FirstOrDefault(item => item.Id == id);

            if (metric is not null)
            {
                metric.Name = newMetric.Name;
                metric.Datasource = newMetric.Datasource;
                metric.Query = newMetric.Query;
                return Task.FromResult((Metric?)metric);
            }
            return Task.FromResult((Metric?)null);
        }

        public Task<IList<Metric>> GetAllAsync()
        {
            return Task.FromResult(_metricList);
        }

        public Task<Metric?> GetByIdAsync(int id)
        {
            return Task.FromResult(_metricList.FirstOrDefault(item => item.Id == id));
        }
    }
}