namespace Service;

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

    private Company TryToGetCompany(Guid companyId, bool trackChanges) =>
        _companies.GetCompany(companyId, trackChanges) ??
        throw new CompanyNotFoundException(companyId);
    
    private Employee TryToGetEmployee(Guid companyId, Guid employeeId, bool trackChanges) =>
        _employees.GetCompanyEmployee(companyId, employeeId, trackChanges) ??
        throw new EmployeeNotFoundException(employeeId);
}