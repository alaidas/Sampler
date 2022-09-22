using FluentAssertions;
using NUnit.Framework;
using Sampler.Models;

namespace Sampler.Tests
{
    [TestFixture]
    public class SamplerTests
    {
        private ISampler _sampler;

        [SetUp]
        public void Setup()
        {
            _sampler = new Sampler();
        }

        [Test]
        public void Sample_Dry_Run()
        {
            var output = _sampler.Sample(DateTime.MinValue, Array.Empty<Measurement>());

            output.Should().NotBeNull();
            output.Count.Should().Be(0);
        }

        [Test]
        public void Sample_Returns_One_Sample()
        {
            var input = new[]
            {
                new Measurement(new DateTime(2017,01,3,10,3,45), 35.79, MeasurementType.Temperature),
                new Measurement(new DateTime(2017,01,3,10,4,45), 36, MeasurementType.Temperature),
                new Measurement(new DateTime(2017,01,3,10,4,50), 36.10, MeasurementType.Temperature)
            };

            var output = _sampler.Sample(DateTime.MinValue, input);
            output.Count.Should().Be(1);

            var measurements = output.Single().Value.ToArray();
            measurements.Length.Should().Be(1);

            var measurement = measurements[0];
            measurement.MeasurementValue.Should().Be(36.10);
        }

        [Test]
        public void Sample_Returns_Three_Groups()
        {
            var input = new[]
            {
                new Measurement(new DateTime(2017,01,3,10,3,45), 35.79, MeasurementType.Temperature),
                new Measurement(new DateTime(2017,01,3,10,4,45), 36, MeasurementType.Temperature),
                new Measurement(new DateTime(2017,01,3,10,4,50), 90, MeasurementType.SpO2),
                new Measurement(new DateTime(2017,01,3,10,4,52), 91, MeasurementType.SpO2),
                new Measurement(new DateTime(2017,01,10,10,4,45), 81, MeasurementType.HeartRate),
                new Measurement(new DateTime(2017,01,10,9,4,45), 75, MeasurementType.HeartRate),
            };

            var output = _sampler.Sample(DateTime.MinValue, input);
            output.Count.Should().Be(3);

            var tempMeasurements = output[MeasurementType.Temperature].ToArray();
            tempMeasurements.Length.Should().Be(1);
            tempMeasurements[0].MeasurementValue.Should().Be(36);

            var spO2Measurements = output[MeasurementType.SpO2].ToArray();
            spO2Measurements.Length.Should().Be(1);
            spO2Measurements[0].MeasurementValue.Should().Be(91);

            var heartRateMeasurements = output[MeasurementType.HeartRate].ToArray();
            heartRateMeasurements.Length.Should().Be(2);
            heartRateMeasurements[0].MeasurementValue.Should().Be(75);
            heartRateMeasurements[1].MeasurementValue.Should().Be(81);
        }


        [Test]
        public void Sample_SamplingDate_Is_Later_Than_Samples_Returns_2_Empty_Groups()
        {
            var input = new[]
            {
                new Measurement(new DateTime(2017,01,3,10,3,45), 35.79, MeasurementType.Temperature),
                new Measurement(new DateTime(2017,01,3,10,4,45), 36, MeasurementType.Temperature),
                new Measurement(new DateTime(2017,01,3,10,4,50), 36.10, MeasurementType.Temperature),
                new Measurement(new DateTime(2017,01,3,10,3,34), 96.49, MeasurementType.SpO2),
            };

            var output = _sampler.Sample(DateTime.MaxValue, input);

            output.Count.Should().Be(2);

            output
                .Select(g => g.Value.ToArray().Length)
                .Sum().Should().Be(0);
        }

        [Test]
        public void Sample_Example_TestCase_Returns_Four_Samples()
        {
            var input = new[]
            {
                new Measurement(new DateTime(2017,01,3,10,4,45), 35.79, MeasurementType.Temperature),
                new Measurement(new DateTime(2017,01,3,10,1,18), 98.78, MeasurementType.SpO2),
                new Measurement(new DateTime(2017,01,3,10,9,7), 35.01, MeasurementType.Temperature),
                new Measurement(new DateTime(2017,01,3,10,3,34), 96.49, MeasurementType.SpO2),
                new Measurement(new DateTime(2017,01,3,10,2,1), 35.82, MeasurementType.Temperature),
                new Measurement(new DateTime(2017,01,3,10,5,00), 97.17, MeasurementType.SpO2),
                new Measurement(new DateTime(2017,01,3,10,5,1), 95.08, MeasurementType.SpO2),
            };

            var output = _sampler.Sample(DateTime.MinValue, input);

            foreach (var measurement in output.SelectMany(group => group.Value))
                Console.WriteLine(measurement);

            output.Count.Should().Be(2);

            var tempMeasurements = output[MeasurementType.Temperature].ToArray();
            tempMeasurements.Length.Should().Be(2);
            tempMeasurements[0].MeasurementValue.Should().Be(35.79);
            tempMeasurements[1].MeasurementValue.Should().Be(35.01);

            var spO2Measurements = output[MeasurementType.SpO2].ToArray();
            spO2Measurements.Length.Should().Be(2);
            spO2Measurements[0].MeasurementValue.Should().Be(97.17);
            spO2Measurements[1].MeasurementValue.Should().Be(95.08);
        }
    }
}