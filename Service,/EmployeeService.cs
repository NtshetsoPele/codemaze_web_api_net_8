namespace Service_;

internal sealed class EmployeeService(IRepositoryManager repository, ILoggerManager logger) : IEmployeeService
{
    private readonly IRepositoryManager _repository = repository; 
    private readonly ILoggerManager _logger = logger;
}