using System;
using System.Collections.Generic;
using AnomalyDetection.Data.Model.Api;
using k8s.Models;

namespace AnomalyDetection.Core.Extension.Model.Api
{
    public static class ApiTrainingJobEx
    {
        public static void ThrowIfNull(this object obj)
        {
            if (obj == null)
                throw new NullReferenceException();
        }

        public static string GetCronJobName(this ApiTrainingJob trainingJob)
        {
            // in case of null metric or name it should throw exception.
            trainingJob.ThrowIfNull();
            trainingJob.Metric.ThrowIfNull();
            trainingJob.Metric.Name.ThrowIfNull();
            return $"TrainingJob-{trainingJob.Metric.Name}".Replace("_", "-").ToLower();
        }

        public static V1CronJob ToCronJob(this ApiTrainingJob trainingJob, string image, string imagePullPolicy, string restartPolicy, IList<V1EnvVar> envList, IList<string> argList)
        {
            envList.Add(new()
            {
                Name = "QUERY", // TODO choose between Query and QUERY
                Value = trainingJob.Metric.Query
            });

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
                                            Env = envList,
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