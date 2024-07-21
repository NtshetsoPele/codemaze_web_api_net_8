namespace Repository;

public class EmployeeRepository(RepositoryContext context) : RepositoryBase<Employee>(context), IEmployeeRepository
{
    public async Task<IEnumerable<Employee>> GetCompanyEmployeesAsync(Guid companyId, bool trackChanges)
    {
        return await FindByCondition((Employee e) => e.CompanyId.Equals(companyId), trackChanges: false)
            .OrderBy((Employee e) => e.Name).ToListAsync();
    }

    public async Task<Employee?> GetCompanyEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        return await FindByCondition(
                (Employee e) => e.CompanyId.Equals(companyId) && e.EmployeeId.Equals(employeeId), 
                trackChanges)
            .SingleOrDefaultAsync();
    }

    public void CreateCompanyEmployee(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteCompanyEmployee(Employee employee) => Delete(employee);
}