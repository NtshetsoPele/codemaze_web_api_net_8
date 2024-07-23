namespace Repository;

public class EmployeeRepository(RepositoryContext context) : RepositoryBase<Employee>(context), IEmployeeRepository
{
    public async Task<PagedList<Employee>> GetCompanyEmployeesAsync(
        Guid companyId, EmployeeParameters parameters, bool trackChanges)
    {
        List<Employee> employees = await GetEmployees(companyId, parameters, trackChanges);
        var employeeCount = await GetEmployeeCountAsync(companyId, trackChanges);
        return new PagedList<Employee>(employees, employeeCount, parameters.PageNumber, parameters.PageSize);
    }

    private async Task<List<Employee>> GetEmployees(Guid companyId, EmployeeParameters parameters, bool trackChanges)
    {
        return await FindByCondition((Employee e) => e.CompanyId.Equals(companyId), trackChanges)
            .OrderBy(e => e.Name)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();
    }
    
    private async Task<int> GetEmployeeCountAsync(Guid companyId, bool trackChanges) =>
        await FindByCondition((Employee e) => e.CompanyId.Equals(companyId), trackChanges)
            .CountAsync();

    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<Employee?> GetCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        return await 
            FindByCondition((Employee emp) => IsCompanyEmployee(companyId, employeeId, emp), trackChanges)
                .SingleOrDefaultAsync();
    }

    private static bool IsCompanyEmployee(Guid companyId, Guid employeeId, Employee employee) =>
        employee.CompanyId.Equals(companyId) && employee.EmployeeId.Equals(employeeId);

    public void CreateCompanyEmployee(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteCompanyEmployee(Employee employee) => Delete(employee);
}