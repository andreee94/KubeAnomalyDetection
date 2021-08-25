using System.Collections.Generic;
using AnomalyDetection.Data.Model;
using k8s.Models;

namespace AnomalyDetection.Core.Extension.Model
{
    public static class TrainingJobEx
    {
        public static string GetCronJobName(this TrainingJob trainingJob)
        {
            return $"TrainingJob-{trainingJob.Metric.Name}".ToLower();
        }

        public static V1CronJob ToCronJob(this TrainingJob trainingJob, string image, string imagePullPolicy, string restartPolicy)
        {
            return new V1CronJob
            {
                ApiVersion = $"{V1CronJob.KubeGroup}/{V1CronJob.KubeApiVersion}",
                Kind = V1CronJob.KubeKind,
                Metadata = new()
                {
                    Name = trainingJob.GetCronJobName(),
                    Labels = new Dictionary<string, string>()
                    {
                        { "MetricName", trainingJob.Metric.Name },
                        // { "MetricHash", trainingJob.Metric.Hash() }
                    }
                },
                Spec = new V1CronJobSpec()
                {
                    Schedule = trainingJob.Metric.TrainingSchedule,
                    JobTemplate = new()
                    {
                        Spec = new()
                        {
                            Template = new()
                            {
                                Spec = new()
                                {
                                    RestartPolicy = restartPolicy,
                                    Containers = new List<V1Container>() {
                                        new() {
                                            Name = "trainingjob-container",
                                            Image = image,
                                            ImagePullPolicy = imagePullPolicy,
                                            Env = new List<V1EnvVar>
                                            {
                                                new() {
                                                    Name = "Query",
                                                    Value = trainingJob.Metric.Query
                                                }
                                            },
                                            Command = new List<string> {"sh", "-c"},
                                            Args = new List<string> {"sleep 10 && echo ${Query}"}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}