namespace Service;

public sealed class ServiceManager(
    IRepositoryManager repoManager, ILoggerManager logger, IMapper mapper) : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService = new(() => new CompanyService(repoManager, logger, mapper)); 
    private readonly Lazy<IEmployeeService> _employeeService = new(() => new EmployeeService(repoManager, logger));

    public ICompanyService CompanyService => _companyService.Value; 
    public IEmployeeService EmployeeService => _employeeService.Value;
}