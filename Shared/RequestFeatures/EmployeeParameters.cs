﻿namespace Shared.RequestFeatures;

public sealed class EmployeeParameters : RequestParameters
{
    public EmployeeParameters() => OrderBy = "name";
    public uint MinAge { get; init; }
    public uint MaxAge { get; init; } = int.MaxValue;
    public bool ValidAgeRange => MaxAge > MinAge;
    public string? SearchTerm { get; init; }
}