namespace Service;

internal sealed class EmployeeService(
    IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IEmployeeService
{
    private readonly ICompanyRepository _companies = repository.Company;
    private readonly IEmployeeRepository _employees = repository.Employee;
    
    public async Task<ToClientEmployees> GetCompanyEmployeesAsync(Guid companyId, bool trackChanges)
    {
        _ = await TryToGetCompanyAsync(companyId, trackChanges);

        return mapper.Map<ToClientEmployees>(GetEmployeesAsync());

        #region Nested_Helpers

        async Task<Employees> GetEmployeesAsync() => 
            await _employees.GetCompanyEmployeesAsync(companyId, trackChanges);

        #endregion
    }

    public async Task<ToClientEmployee> GetCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        _ = await TryToGetCompanyAsync(companyId, trackChanges);
        
        var employee = GetEmployeeAsync();

        return ReturnIfEmployeeExistsOrThrow();

        #region Nested_Helpers

        async Task<Employee?> GetEmployeeAsync() =>
            await _employees.GetCompanyEmployeeAsync(companyId, employeeId, trackChanges);

        ToClientEmployee ReturnIfEmployeeExistsOrThrow() =>
            employee is null ?
                throw new EmployeeNotFoundException(employeeId) :
                mapper.Map<ToClientEmployee>(employee);

        #endregion
    }

    public async Task<ToClientEmployee> CreateCompanyEmployeeAsync(
        Guid companyId, EmployeeCreationRequest newEmployee, bool trackChanges)
    {
        _ = await TryToGetCompanyAsync(companyId, trackChanges);
        var domainEmployee = mapper.Map<Employee>(newEmployee);
        _employees.CreateCompanyEmployee(companyId, domainEmployee);
        await repository.SaveAsync();
        return mapper.Map<ToClientEmployee>(domainEmployee);
    }

    public async Task DeleteCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        _ = await TryToGetCompanyAsync(companyId, trackChanges);
        var employee = await TryToGetEmployeeAsync(companyId, employeeId, trackChanges);
        
        _employees.DeleteCompanyEmployee(employee);
        await repository.SaveAsync();
    }

    public async Task UpdateCompanyEmployeeAsync(CompanyEmployeeUpdateParameters empUpdate)
    {
        _ = await TryToGetCompanyAsync(empUpdate.CmpId, empUpdate.CmpTrackChanges);
        var employee = await TryToGetEmployeeAsync(empUpdate.CmpId, empUpdate.EmpId, empUpdate.EmpTrackChanges);
        mapper.Map(empUpdate.EmpUpdate, employee);
        await repository.SaveAsync();
    }

    public async Task<(EmployeeUpdateRequest updateEmp, Employee domainEmp)> GetPatchEmployeeAsync(
        EmployeePatchParameters empPatch)
    {
        _ = await TryToGetCompanyAsync(empPatch.CmpId, empPatch.CmpTrackChanges);
        Employee domainEmp = await TryToGetEmployeeAsync(empPatch.CmpId, empPatch.EmpId, empPatch.EmpTrackChanges);
        var updateEmp = mapper.Map<EmployeeUpdateRequest>(domainEmp);
        return (updateEmp, domainEmp);
    }

    public async Task ApplyPatchAsync(EmployeeUpdateRequest updateEmp, Employee domainEmp)
    {
        mapper.Map(updateEmp, domainEmp);
        await repository.SaveAsync();
    }

    private async Task<Company> TryToGetCompanyAsync(Guid companyId, bool trackChanges) =>
        await _companies.GetCompanyAsync(companyId, trackChanges) ??
        throw new CompanyNotFoundException(companyId);
    
    private async Task<Employee> TryToGetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges) =>
        await _employees.GetCompanyEmployeeAsync(companyId, employeeId, trackChanges) ??
        throw new EmployeeNotFoundException(employeeId);
}