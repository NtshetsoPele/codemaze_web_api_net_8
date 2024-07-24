namespace Contracts;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetCompanyEmployeesAsync(
        Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<Employee?> GetCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateCompanyEmployee(Guid companyId, Employee employee);
    void DeleteCompanyEmployee(Employee employee);
}