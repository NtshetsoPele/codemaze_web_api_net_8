namespace Shared.ParameterObjects;

public sealed class CompanyEmployeeUpdateParameters
{
    public required Guid CmpId { get; init; }
    public required Guid EmpId { get; init; }
    public required EmployeeUpdateRequest EmpUpdate { get; init; }
    public required bool CmpTrackChanges { get; init; }
    public required bool EmpTrackChanges { get; init; }
}