using Sampler.Models;

namespace Sampler
{
    public interface ISampler
    {
        IDictionary<MeasurementType, IEnumerable<Measurement>> Sample(DateTime startOfSampling, IEnumerable<Measurement> unsampledMeasurements);
    }
}
