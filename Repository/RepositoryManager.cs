namespace Repository;

public sealed class RepositoryManager(RepositoryContext context) : IRepositoryManager
{
    private readonly Lazy<ICompanyRepository> _companyRepo = new(() => new CompanyRepository(context)); 
    private readonly Lazy<IEmployeeRepository> _employeeRepo = new(() => new EmployeeRepository(context));

    public ICompanyRepository Company => _companyRepo.Value; 
    public IEmployeeRepository Employee => _employeeRepo.Value;

    /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
    /// <exception cref="DbUpdateConcurrencyException">A concurrency violation is encountered while saving to the database.
    ///                 A concurrency violation occurs when an unexpected number of rows are affected during save.
    ///                 This is usually because the data in the database has been modified since it was loaded into memory.</exception>
    public async Task SaveAsync() => await context.SaveChangesAsync();
}