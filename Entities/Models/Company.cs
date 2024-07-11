namespace Entities.Models;

public sealed class Company
{
    //[Column(name: "CompanyId")]
    public required Guid CompanyId { get; init; }
    
    [Required(ErrorMessage = "Company name is required.")]
    [MaxLength(length: 60, ErrorMessage = "Maximum length is 60 characters.")] 
    public required string Name { get; set; }
    
    [Required(ErrorMessage = "Company address is required.")]
    [MaxLength(length: 60, ErrorMessage = "Maximum length is 60 characters.")] 
    public required string Address { get; set; }
    
    public required string Country { get; set; }
    
    public ICollection<Employee>? Employees { get; set; }
}