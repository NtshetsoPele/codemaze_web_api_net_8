namespace Service;

public sealed class ServiceManager(
    IRepositoryManager repoManager, 
    ILoggerManager logger, 
    IMapper mapper, 
    IDataShaper<ToClientEmployee> dataShaper,
    UserManager<User> userManager, 
    IConfiguration configuration) : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService = new(() => new CompanyService(repoManager, logger, mapper)); 
    private readonly Lazy<IEmployeeService> _employeeService = new(() => new EmployeeService(repoManager, logger, mapper, dataShaper));
    private readonly Lazy<IAuthenticationService> _authenticationService = new(() => new AuthenticationService(logger, mapper, userManager, configuration));

    public ICompanyService CompanyService => _companyService.Value; 
    public IEmployeeService EmployeeService => _employeeService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}