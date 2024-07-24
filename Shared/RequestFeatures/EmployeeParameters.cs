namespace Shared.RequestFeatures;

public sealed class EmployeeParameters : RequestParameters
{
    public uint MinAge { get; init; }
    public uint MaxAge { get; init; } = int.MaxValue;
    public bool ValidAgeRange => MaxAge > MinAge;
}