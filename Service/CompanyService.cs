namespace Service;

internal sealed class CompanyService(
    IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : ICompanyService
{
    private readonly ICompanyRepository _companies = repository.Company;
    
    public ClientCompanies GetAllCompanies(bool trackChanges)
    {
        return mapper.Map<ClientCompanies>(GetCompanies());

        #region Nested_Helpers

        Companies GetCompanies() => _companies.GetAllCompanies(trackChanges);

        #endregion
    }

    /// <exception cref="CompanyNotFoundException">Condition.</exception>
    public ToClientCompany GetCompanyById(Guid id, bool trackChanges)
    {
        var domainCompany = GetCompany();

        return ReturnCompanyIfFound();

        #region Nested_Helpers

        Company? GetCompany() => _companies.GetCompany(id, trackChanges);

        ToClientCompany ReturnCompanyIfFound() =>
            domainCompany is null
                ? throw new CompanyNotFoundException(id)
                : mapper.Map<ToClientCompany>(domainCompany);

        #endregion
    }

    public ToClientCompany CreateCompany(CompanyCreationRequest company)
    {
        Company domainCompany = mapper.Map<Company>(company);
        _companies.CreateCompany(domainCompany);
        repository.Save();
        return mapper.Map<ToClientCompany>(domainCompany);
    }

    /// <exception cref="IdParametersBadRequestException">Condition.</exception>
    /// <exception cref="CollectionByIdsBadRequestException">Condition.</exception>
    public ClientCompanies GetCompaniesByIds(CompanyIds ids, bool trackChanges)
    {
        ThrowIfCompanyIdsIsNull();
        
        List<Guid> companyIdsList = ids.ToList();
        Companies domainCompanies = GetCompanies();

        ThrowIfCompanyCountsMismatch();
        
        return mapper.Map<ClientCompanies>(domainCompanies);

        #region Nested_Helpers

        void ThrowIfCompanyIdsIsNull()
        {
            if (ids is null)
            {
                throw new IdParametersBadRequestException();
            }
        }
        
        Companies GetCompanies() =>
            _companies.GetCompaniesByTheirIds(companyIdsList, trackChanges);

        void ThrowIfCompanyCountsMismatch()
        {
            if (companyIdsList.Count != domainCompanies.Count())
            {
                throw new CollectionByIdsBadRequestException();
            }
        }
        
        #endregion
    }

    /// <exception cref="OutOfMemoryException">The length of the resulting string overflows the maximum allowed length (<see cref="System.Int32.MaxValue">Int32.MaxValue</see>).</exception>
    /// <exception cref="CompanyCollectionBadRequest">Condition.</exception>
    public (ClientCompanies clientCompanies, string ids) CreateCompanyCollection(NewCompanies? newCompanies)
    {
        if (newCompanies is null)
        {
            throw new CompanyCollectionBadRequest();
        }

        Companies companies = mapper.Map<Companies>(newCompanies);
        foreach (var company in companies)
        {
            _companies.CreateCompany(company);
        }
        repository.Save();

        var clientCompanies = mapper.Map<ClientCompanies>(companies).ToList();
        var companyIds = string.Join(",", clientCompanies.Select((ToClientCompany c) => c.CompanyId));

        return (clientCompanies, companyIds);
    }

    public void DeleteCompanyById(Guid companyId, bool trackChanges)
    {
        var domainCompany = TryToGetCompany(companyId, trackChanges);
        _companies.DeleteCompany(domainCompany);
        repository.Save();
    }

    public void UpdateCompany(Guid companyId, CompanyUpdateRequest companyUpdate, bool trackChanges)
    {
        var domainCompany = TryToGetCompany(companyId, trackChanges);
        mapper.Map(companyUpdate, domainCompany);
        repository.Save();
    }
    
    Company TryToGetCompany(Guid companyId, bool trackChanges) =>
        _companies.GetCompany(companyId, trackChanges) ??
        throw new CompanyNotFoundException(companyId);
}