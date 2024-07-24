namespace Repository;

public class EmployeeRepository(RepositoryContext context) : RepositoryBase<Employee>(context), IEmployeeRepository
{
    public async Task<PagedList<Employee>> GetCompanyEmployeesAsync(
        Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        List<Employee> employees = await GetEmployees(companyId, employeeParameters, trackChanges);
        int employeeCount = await GetEmployeeCountAsync(companyId, employeeParameters, trackChanges);
        return new PagedList<Employee>(
            employees, 
            employeeCount, 
            employeeParameters.PageNumber, 
            employeeParameters.PageSize);
    }

    private async Task<List<Employee>> GetEmployees(
        Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        return await 
            FindByCondition((Employee e) => e.CompanyId.Equals(companyId), trackChanges)
                .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
                .Search(employeeParameters.SearchTerm)
                .Sort(employeeParameters.OrderBy)
                .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
                .Take(employeeParameters.PageSize)
                .ToListAsync();
    }
    
    private async Task<int> GetEmployeeCountAsync(
        Guid companyId, EmployeeParameters employeeParameters, bool trackChanges) =>
        await 
            FindByCondition((Employee e) => e.CompanyId.Equals(companyId), trackChanges)
                .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
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