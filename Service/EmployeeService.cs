namespace Service;

internal sealed class EmployeeService(
    IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : IEmployeeService
{
    public IEnumerable<ToClientEmployee> GetCompanyEmployees(Guid companyId, bool trackChanges)
    {
        Company? company = repository.Company.GetCompany(companyId, trackChanges);
        
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        IEnumerable<Employee> domainEmployees = repository.Employee.GetCompanyEmployees(companyId, trackChanges);
        
        return mapper.Map<IEnumerable<ToClientEmployee>>(domainEmployees);
    }

    public ToClientEmployee GetCompanyEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        Company? company = repository.Company.GetCompany(companyId, trackChanges: false);
        
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        Employee? employee = repository.Employee.GetCompanyEmployee(companyId, employeeId, trackChanges: false);
        
        if (employee is null)
        {
            throw new EmployeeNotFoundException(employeeId);
        }

        return mapper.Map<ToClientEmployee>(employee);
    }
}