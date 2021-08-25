namespace AnomalyDetection.Data.Model.Option
{
    public class TrainingJobOptions
    {
        public TrainingJobOptions()
        {
            ImagePullPolicy = "Always";
        }
        public TrainingJobOptions(string image, string kubeNamespace, string? imagePullPolicy, bool startImmediate, string restartPolicy)
        {
            Image = image;
            KubeNamespace = kubeNamespace;
            ImagePullPolicy = imagePullPolicy ?? "Always";
            StartImmediate = startImmediate;
            RestartPolicy = restartPolicy;
        }

        public string Image { get; set; }
        public string ImagePullPolicy { get; set; }
        public string KubeNamespace { get; set; }
        public string RestartPolicy { get; set; }
        public bool StartImmediate { get; set; }
    }
}