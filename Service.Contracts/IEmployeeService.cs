namespace Service.Contracts;

public interface IEmployeeService
{
    ClientEmployees GetCompanyEmployees(Guid companyId, bool trackChanges);
    ToClientEmployee GetCompanyEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    ToClientEmployee CreateCompanyEmployee(Guid companyId, EmployeeCreationRequest newEmployee, bool trackChanges);
    void DeleteCompanyEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    void UpdateCompanyEmployee(CompanyEmployeeUpdateParameters empUpdate);
}