namespace Contracts;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetCompanyEmployeesAsync(Guid companyId, bool trackChanges);
    Task<Employee?> GetCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateCompanyEmployee(Guid companyId, Employee employee);
    void DeleteCompanyEmployee(Employee employee);
}