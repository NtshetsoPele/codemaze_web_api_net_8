﻿namespace Service;

internal sealed class EmployeeService(
    IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IEmployeeService
{
    private readonly ICompanyRepository _companies = repository.Company;
    private readonly IEmployeeRepository _employees = repository.Employee;
    
    public ToClientEmployees GetCompanyEmployees(Guid companyId, bool trackChanges)
    {
        _ = TryToGetCompany(companyId, trackChanges);

        return mapper.Map<ToClientEmployees>(GetEmployees());

        #region Nested_Helpers

        Employees GetEmployees() => 
            _employees.GetCompanyEmployees(companyId, trackChanges);

        #endregion
    }

    public ToClientEmployee GetCompanyEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        _ = TryToGetCompany(companyId, trackChanges);
        
        var employee = GetEmployee();

        return ReturnIfEmployeeExistsOrThrow();

        #region Nested_Helpers

        Employee? GetEmployee() =>
            _employees.GetCompanyEmployee(companyId, employeeId, trackChanges);

        ToClientEmployee ReturnIfEmployeeExistsOrThrow() =>
            employee is null ?
                throw new EmployeeNotFoundException(employeeId) :
                mapper.Map<ToClientEmployee>(employee);

        #endregion
    }

    public ToClientEmployee CreateCompanyEmployee(
        Guid companyId, EmployeeCreationRequest newEmployee, bool trackChanges)
    {
        _ = TryToGetCompany(companyId, trackChanges);
        var domainEmployee = mapper.Map<Employee>(newEmployee);
        _employees.CreateCompanyEmployee(companyId, domainEmployee);
        repository.Save();
        return mapper.Map<ToClientEmployee>(domainEmployee);
    }

    public void DeleteCompanyEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        _ = TryToGetCompany(companyId, trackChanges);
        var employee = TryToGetEmployee(companyId, employeeId, trackChanges);
        
        _employees.DeleteCompanyEmployee(employee);
        repository.Save();
    }

    public void UpdateCompanyEmployee(CompanyEmployeeUpdateParameters empUpdate)
    {
        _ = TryToGetCompany(empUpdate.CmpId, empUpdate.CmpTrackChanges);
        var employee = TryToGetEmployee(empUpdate.CmpId, empUpdate.EmpId, empUpdate.EmpTrackChanges);
        mapper.Map(empUpdate.EmpUpdate, employee);
        repository.Save();
    }

    public (EmployeeUpdateRequest updateEmp, Employee domainEmp) GetPatchEmployee(EmployeePatchParameters empPatch)
    {
        _ = TryToGetCompany(empPatch.CmpId, empPatch.CmpTrackChanges);
        Employee domainEmp = TryToGetEmployee(empPatch.CmpId, empPatch.EmpId, empPatch.EmpTrackChanges);
        var updateEmp = mapper.Map<EmployeeUpdateRequest>(domainEmp);
        return (updateEmp, domainEmp);
    }

    public void ApplyPatch(EmployeeUpdateRequest updateEmp, Employee domainEmp)
    {
        mapper.Map(updateEmp, domainEmp);
        repository.Save();
    }

    private Company TryToGetCompany(Guid companyId, bool trackChanges) =>
        _companies.GetCompany(companyId, trackChanges) ??
        throw new CompanyNotFoundException(companyId);
    
    private Employee TryToGetEmployee(Guid companyId, Guid employeeId, bool trackChanges) =>
        _employees.GetCompanyEmployee(companyId, employeeId, trackChanges) ??
        throw new EmployeeNotFoundException(employeeId);
}