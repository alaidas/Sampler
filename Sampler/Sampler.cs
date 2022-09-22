using Sampler.Models;

namespace Sampler
{
    public class Sampler : ISampler
    {
        private static readonly TimeSpan SamplingTimeInterval = TimeSpan.FromMinutes(5);    //Could be moved to configuration or to constructor

        public IDictionary<MeasurementType, IEnumerable<Measurement>> Sample(DateTime startOfSampling, IEnumerable<Measurement> unsampledMeasurements)
        {
            var result = new Dictionary<MeasurementType, IEnumerable<Measurement>>();

            var groupedByType = unsampledMeasurements.GroupBy(m => m.MeasurementType);
            foreach (var group in groupedByType)
            {
                var samplingFrame = group.Where(m => m.MeasurementTime > startOfSampling);

                var groupedBySamplingTimeFrame = samplingFrame
                    .GroupBy(m => GetSamplingTimeKey(m.MeasurementTime, SamplingTimeInterval));

                var sampledGroup = groupedBySamplingTimeFrame
                    .Select(g =>
                        g.MaxBy(m => m.MeasurementTime)!);

                var orderedSampledGroup = sampledGroup
                    .OrderBy(m => m.MeasurementTime);

                result[group.Key] = orderedSampledGroup;
            }

            return result;
        }

        private static DateTime GetSamplingTimeKey(DateTime dateTime, TimeSpan samplingTime)
            => new((dateTime.Ticks + samplingTime.Ticks - 1) / samplingTime.Ticks * samplingTime.Ticks);
    }
}
