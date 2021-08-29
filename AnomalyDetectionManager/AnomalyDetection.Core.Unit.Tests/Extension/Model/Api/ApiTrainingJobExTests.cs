using AnomalyDetection.Data.Model.Api;
using Tynamix.ObjectFiller;

namespace AnomalyDetection.Core.Unit.Tests.Extension.Model.Api
{
    public partial class ApiTrainingJobExTests
    {
        private static ApiTrainingJob CreateRandomTrainingJob()
        {
            return new Filler<ApiTrainingJob>().Create();
        }
    }
}