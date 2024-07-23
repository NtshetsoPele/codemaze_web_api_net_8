namespace Service.Contracts;

public interface IEmployeeService
{
    Task<(ClientEmployees employees, MetaData metaData)> GetCompanyEmployeesAsync(
        Guid companyId, EmployeeParameters parameters, bool trackChanges);
    Task<ToClientEmployee> GetCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    Task<ToClientEmployee> CreateCompanyEmployeeAsync(Guid companyId, EmployeeCreationRequest newEmployee, bool trackChanges);
    Task DeleteCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    Task UpdateCompanyEmployeeAsync(CompanyEmployeeUpdateParameters empUpdate);
    Task<(EmployeeUpdateRequest updateEmp, Employee domainEmp)> GetPatchEmployeeAsync(EmployeePatchParameters empPatch);
    Task ApplyPatchAsync(EmployeeUpdateRequest updateEmp, Employee domainEmp);
}