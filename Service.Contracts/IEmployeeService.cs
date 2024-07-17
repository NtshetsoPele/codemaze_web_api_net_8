namespace Service.Contracts;

public interface IEmployeeService
{
    IEnumerable<ToClientEmployee> GetCompanyEmployees(Guid companyId, bool trackChanges);
    ToClientEmployee GetCompanyEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    ToClientEmployee CreateCompanyEmployee(Guid companyId, EmployeeCreationRequest newEmployee, bool trackChanges);
}