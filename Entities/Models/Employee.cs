namespace Entities.Models;

public sealed class Employee
{
    public required Guid EmployeeId { get; init; }
        
    [Required(ErrorMessage = "Employee name is required.")]
    [MaxLength(length: 30, ErrorMessage = "Maximum length is 30 characters.")] 
    public required string Name { get; set; }
        
    [Required(ErrorMessage = "Age is required.")] 
    public required int Age { get; set; }
        
    [Required(ErrorMessage = "Position is required.")]
    [MaxLength(length: 20, ErrorMessage = "Maximum length is 20 characters.")] 
    public required string Position { get; set; }
        
    [ForeignKey(nameof(Company))] 
    public required Guid CompanyId { get; set; }
    public Company? Company { get; set; }
}