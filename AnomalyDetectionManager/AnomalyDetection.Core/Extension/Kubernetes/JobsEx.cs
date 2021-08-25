using System;
using System.Collections.Generic;
using k8s.Models;

namespace AnomalyDetection.Core.Extension.Kubernetes
{
    public static class JobsEx
    {
        public static V1Job ToManualJob(this V1CronJob cronJob)
        {
            // random 8 digit number to be appended to the pod name
            string rand = new Random().Next((int)1e8, (int)1e9).ToString("00000000");
            return new V1Job()
            {
                ApiVersion = $"{V1Job.KubeGroup}/{V1Job.KubeApiVersion}",
                Kind = V1Job.KubeKind,
                Status = new V1JobStatus(),
                Metadata = new()
                {
                    Name = $"{cronJob.Metadata.Name}-manual-{rand}", //TODO
                    Labels = cronJob.Metadata.Labels,
                    Annotations = new Dictionary<string, string>()
                    {
                        // This annotation is added by kubectl, probably best to add it ourselves as well
                        { "cronjob.kubernetes.io/instantiate", "manual" }
                    }
                },
                Spec = cronJob.Spec.JobTemplate.Spec
            };
        }
    }
}