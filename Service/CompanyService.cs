namespace Service;

internal sealed class CompanyService(
    IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : ICompanyService
{
    public IEnumerable<ToClientCompany> GetAllCompanies(bool trackChanges)
    {
        IEnumerable<Company> domainCompanies = repository.Company.GetAllCompanies(trackChanges);
        return mapper.Map<IEnumerable<ToClientCompany>>(domainCompanies);
    }

    public ToClientCompany GetCompanyById(Guid companyId, bool trackChanges)
    {
        Company? domainCompany = repository.Company.GetCompany(companyId, trackChanges);

        return domainCompany is not null
            ? mapper.Map<ToClientCompany>(domainCompany)
            : throw new CompanyNotFoundException(companyId);
    }
}