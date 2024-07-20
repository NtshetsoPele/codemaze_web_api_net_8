namespace Shared.DTOs.Requests;

public record EmployeeCreationRequest
{
    [Required(ErrorMessage = "Employee name can't be empty.")]
    [MaxLength(length: 30, ErrorMessage = "Employee name can't be longer than 30 characters.")]
    public required string Name { get; init; }
    
    [Range(minimum: 18, maximum: byte.MaxValue, ErrorMessage = "Employee age must be a minimum of 18.")]
    public required int Age { get; init; }
    
    [Required(ErrorMessage = "Employee position is required.")]
    [MaxLength(length: 30, ErrorMessage = "Employee position can't be longer than 30 characters.")]
    public required string Position { get; init; }
}