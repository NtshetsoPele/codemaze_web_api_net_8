namespace Shared.DTOs.Responses;

public record ToClientEmployee
{
    public required Guid EmployeeId { get; init; }
    public required string Name { get; set; }
    public required int Age { get; set; }
    public required string Position { get; set; }
}