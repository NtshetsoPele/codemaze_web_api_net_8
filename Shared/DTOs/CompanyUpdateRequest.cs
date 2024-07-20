namespace Shared.DTOs;

public record CompanyUpdateRequest(string Name, string Address, string Country, EmpUpdates Employees);