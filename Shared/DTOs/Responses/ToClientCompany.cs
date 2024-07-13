namespace Shared.DTOs.Responses;

public class ToClientCompany
{
    public required Guid CompanyId { get; init; }
    public required string Name { get; init; }
    public required string FullAddress { get; init; }
}