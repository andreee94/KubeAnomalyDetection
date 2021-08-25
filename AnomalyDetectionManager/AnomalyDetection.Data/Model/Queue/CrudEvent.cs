namespace AnomalyDetection.Data.Model.Queue
{
    public class CrudEvent<T>
    {
        public CrudAction Action { get; set; }

        public T Item { get; set; }
    }
}