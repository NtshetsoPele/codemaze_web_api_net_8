namespace Shared.ParameterObjects;

public abstract class UpdateParameters
{
    public required Guid CmpId { get; init; }
    public required Guid EmpId { get; init; }
    public required bool CmpTrackChanges { get; init; }
    public required bool EmpTrackChanges { get; init; }
}