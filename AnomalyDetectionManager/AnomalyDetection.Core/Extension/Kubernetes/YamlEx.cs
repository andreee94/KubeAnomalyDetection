using k8s;
using k8s.Models;
using Microsoft.Extensions.Logging;

namespace AnomalyDetection.Core.Extension.Kubernetes
{
    public static class YamlEx
    {
        public static void LogDebug<T>(this T resource, ILogger logger)
        {
            var yaml = Yaml.SaveToString(resource);
            logger.LogDebug(yaml);
        }
    }
}