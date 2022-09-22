# Sampler

In general this is a library which could be used in any project. Single interface `ISampler` should be used.

### This is the code snippet for use:

```csharp
ISampler sampler = new Sampler();

var input = new[]
{
    new Measurement(new DateTime(2017,01,3,10,3,45), 35.79, MeasurementType.Temperature),
    new Measurement(new DateTime(2017,01,3,10,4,45), 36, MeasurementType.Temperature),
    new Measurement(new DateTime(2017,01,3,10,4,50), 36.10, MeasurementType.Temperature)
};

var output = sampler.Sample(DateTime.MinValue, input);
```

### Couple notes

Changed initial start method from:
```csharp
Dictionary<MeasurementType, List<Measurement>> sample(DateTime startOfSampling, List<Measurement> unsampledMeasurements)
```

to more general approach and according C# naming conventions:
```csharp
IDictionary<MeasurementType, IEnumerable<Measurement>> Sample(DateTime startOfSampling, IEnumerable<Measurement> unsampledMeasurements)
```
This should allow optimizations done by compiler/runtime and etc.