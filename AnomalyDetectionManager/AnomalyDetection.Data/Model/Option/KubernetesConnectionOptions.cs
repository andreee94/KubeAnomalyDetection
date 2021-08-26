namespace AnomalyDetection.Data.Model.Option
{
    public class KubernetesConnectionOptions
    {
        public string KubeConfigPath { get; set; }
        public bool InCluster { get; set; }
    }
}