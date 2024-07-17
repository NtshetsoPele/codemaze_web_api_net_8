namespace Service;

internal sealed class CompanyService(
    IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : ICompanyService
{
    public ClientCompanies GetAllCompanies(bool trackChanges)
    {
        return mapper.Map<ClientCompanies>(GetCompanies());

        IEnumerable<Company> GetCompanies() => 
            repository.Company.GetAllCompanies(trackChanges);
    }

    public ToClientCompany GetCompanyById(Guid companyId, bool trackChanges)
    {
        var domainCompany = GetCompany();

        return domainCompany is null
            ? throw new CompanyNotFoundException(companyId)
            : mapper.Map<ToClientCompany>(domainCompany);
        
        Company? GetCompany() =>
            repository.Company.GetCompany(companyId, trackChanges);
    }

    public ToClientCompany CreateCompany(CompanyCreationRequest company)
    {
        Company domainCompany = mapper.Map<Company>(company);
        repository.Company.CreateCompany(domainCompany);
        repository.Save();
        return mapper.Map<ToClientCompany>(domainCompany);
    }
}