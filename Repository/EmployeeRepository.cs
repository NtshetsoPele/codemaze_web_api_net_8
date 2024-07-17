namespace Repository;

public class EmployeeRepository(RepositoryContext context) : RepositoryBase<Employee>(context), IEmployeeRepository
{
    public IEnumerable<Employee> GetCompanyEmployees(Guid companyId, bool trackChanges)
    {
        return [.. FindByCondition((Employee e) => e.CompanyId.Equals(companyId), trackChanges: false)
            .OrderBy((Employee e) => e.Name)];
    }

    public Employee? GetCompanyEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        return FindByCondition(
                (Employee e) => e.CompanyId.Equals(companyId) && e.EmployeeId.Equals(employeeId), 
                trackChanges)
            .SingleOrDefault();
    }

    public void CreateCompanyEmployee(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }
}