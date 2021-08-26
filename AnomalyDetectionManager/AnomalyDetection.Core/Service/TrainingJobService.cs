using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnomalyDetection.Core.Extension.Kubernetes;
using AnomalyDetection.Core.Extension.Model;
using AnomalyDetection.Data.Model;
using AnomalyDetection.Data.Model.Option;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AnomalyDetection.Core.Service
{
    public class TrainingJobService
    {
        private readonly ILogger<TrainingJobService> _logger;
        private readonly IKubernetes _kubernetesClient;
        private readonly TrainingJobOptions _options;

        public TrainingJobService(ILogger<TrainingJobService> logger, IKubernetes kubernetesClient, IOptions<TrainingJobOptions> trainingJobOptions)
        {
            _logger = logger;
            _kubernetesClient = kubernetesClient;
            _options = trainingJobOptions.Value;
        }

        public async Task CreateCronJob(TrainingJob trainingJob)
        {
            await CreateNamespaceIfNotExistsAsync(_options.KubeNamespace).ConfigureAwait(false);

            var cronJob = trainingJob.ToCronJob(_options.Image, _options.ImagePullPolicy, _options.RestartPolicy, _options.Env, _options.Args);

            await CreateCronjob(cronJob, await ExistsCronJob(cronJob).ConfigureAwait(false)).ConfigureAwait(false);

            if (_options.StartImmediate)
            {
                var manualJob = cronJob.ToManualJob();
                if (!await ExistsAndRunningManualJob(manualJob).ConfigureAwait(false))
                {
                    await StartManualJob(manualJob).ConfigureAwait(false);
                }
            }
        }

        public async Task CreateNamespaceIfNotExistsAsync(string kubeNamespace)
        {
            var namespaceList = await _kubernetesClient.ListNamespaceAsync().ConfigureAwait(false);

            if (!namespaceList.Items.Any(ns => ns.Metadata.Name == kubeNamespace))
            {
                _logger.LogInformation($"Creating namespace {kubeNamespace} that does not exist yet");
                V1Namespace ns = new()
                {
                    Metadata = new V1ObjectMeta
                    {
                        Name = _options.KubeNamespace
                    }
                };
                ns.LogDebugYaml(_logger);
                await _kubernetesClient.CreateNamespaceAsync(ns).ConfigureAwait(false);
            }
            else
            {
                _logger.LogDebug($"Not creating namespace {kubeNamespace} which already exists");
            }
        }

        public async Task DeleteCronJob(TrainingJob item)
        {
            var cronJobName = item.GetCronJobName();
            if (await ExistsCronJob(cronJobName).ConfigureAwait(false))
            {
                _logger.LogInformation($"Delete CronJob {cronJobName}");
                await _kubernetesClient.DeleteNamespacedCronJobAsync(cronJobName, _options.KubeNamespace).ConfigureAwait(false);
            }
            else
            {
                _logger.LogInformation($"CronJob {cronJobName} does not exist. Not deleting it");
            }
        }

        public async Task<bool> ExistsCronJob(V1CronJob cronJob) => await ExistsCronJob(cronJob.Metadata.Name).ConfigureAwait(false);

        public async Task<bool> ExistsCronJob(string cronJobName)
        {
            var cronJobList = await _kubernetesClient.ListNamespacedCronJobAsync(_options.KubeNamespace, null, null).ConfigureAwait(false);

            return cronJobList.Items.Any(cj => cj.Metadata.Name == cronJobName);

            // var metricHash = trainingJob.Metric.Hash();
            // return cronJobList.Items.Any(cronJob => cronJob.Metadata.Labels.ContainsKey("MetricHash") && cronJob.Metadata.Labels["MetricHash"] == metricHash);
        }

        public async Task<bool> ExistsAndRunningManualJob(V1Job job) => await ExistsAndRunningManualJob(job.Metadata.Name).ConfigureAwait(false);

        public async Task<bool> ExistsAndRunningManualJob(string jobName)
        {
            var jobList = await _kubernetesClient.ListNamespacedJobAsync(_options.KubeNamespace).ConfigureAwait(false);

            return jobList.Items.Any(j => j.Metadata.Name == jobName && j.Status.CompletionTime is null);

            // var metricHash = trainingJob.Metric.Hash();
            // return jobList.Items.Any(job => job.Metadata.Labels.ContainsKey("MetricHash") && job.Metadata.Labels["MetricHash"] == metricHash);
        }

        public async Task CreateCronjob(V1CronJob cronJob, bool replace)
        {
            _logger.LogInformation($"Creating cronjob {cronJob.Metadata.Name}");
            cronJob.LogDebugYaml(_logger);
            if (replace)
            {
                // await _kubernetesClient.ReplaceNamespacedCronJobAsync(cronJob, cronJob.Metadata.Name, _options.KubeNamespace)
                //         .ConfigureAwait(false);
                // DELETE instead of replace to always guarantee the update
                await _kubernetesClient.DeleteNamespacedCronJobAsync(cronJob.Metadata.Name, _options.KubeNamespace).ConfigureAwait(false);
            }
            await _kubernetesClient.CreateNamespacedCronJobAsync(cronJob, _options.KubeNamespace).ConfigureAwait(false);
        }

        public async Task StartManualJob(V1Job manualJob)
        {
            _logger.LogInformation($"Creating manualjob {manualJob.Metadata.Name}");
            manualJob.LogDebugYaml(_logger);
            await _kubernetesClient.CreateNamespacedJobAsync(manualJob, _options.KubeNamespace).ConfigureAwait(false);
        }
    }
}