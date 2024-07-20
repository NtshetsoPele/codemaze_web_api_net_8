namespace Shared.DTOs.Requests;

public record CompanyCreationRequest(
    string Name, 
    string Address, 
    string Country, 
    IEnumerable<EmployeeCreationRequest> Employees);