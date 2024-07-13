namespace Service;

internal sealed class CompanyService(
    IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : ICompanyService
{
    public IEnumerable<ToClientCompany> GetAllCompanies(bool trackChanges)
    {
        IEnumerable<Company> domainCompanies = repository.Company.GetAllCompanies(trackChanges);
        return mapper.Map<IEnumerable<ToClientCompany>>(domainCompanies);
    }
}