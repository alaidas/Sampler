using System.Globalization;

namespace Sampler.Models
{
    public record Measurement(
        DateTime MeasurementTime,
        double MeasurementValue,
        MeasurementType MeasurementType)
    {
        public override string ToString()
        {
            return $"{MeasurementTime.ToString("G", CultureInfo.InvariantCulture)}, {MeasurementType}, {MeasurementValue}";
        }
    }
}