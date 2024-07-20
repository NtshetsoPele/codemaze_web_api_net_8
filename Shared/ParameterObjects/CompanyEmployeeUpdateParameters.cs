namespace Shared.ParameterObjects;

public sealed class CompanyEmployeeUpdateParameters : UpdateParameters
{
    public required EmployeeUpdateRequest EmpUpdate { get; init; }
}