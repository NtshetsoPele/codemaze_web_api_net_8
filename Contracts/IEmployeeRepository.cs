namespace Contracts;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetCompanyEmployees(Guid companyId, bool trackChanges);
    Employee? GetCompanyEmployee(Guid companyId, Guid employeeId, bool trackChanges);
}