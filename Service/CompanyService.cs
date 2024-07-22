namespace Service;

internal sealed class CompanyService(
    IRepositoryManager repository, ILoggerManager logger, IMapper mapper) : ICompanyService
{
    private readonly ICompanyRepository _companies = repository.Company;
    
    public async Task<ClientCompanies> GetAllCompaniesAsync(bool trackChanges)
    {
        return mapper.Map<ClientCompanies>(await GetCompaniesAsync());

        #region Nested_Helpers

        async Task<Companies> GetCompaniesAsync() => 
            await _companies.GetAllCompaniesAsync(trackChanges);

        #endregion
    }

    /// <exception cref="CompanyNotFoundException">Condition.</exception>
    public async Task<ToClientCompany> GetCompanyByIdAsync(Guid id, bool trackChanges)
    {
        var domainCompany = await TryToGetCompanyAsync(id, trackChanges);

        return mapper.Map<ToClientCompany>(domainCompany);
    }

    public async Task<ToClientCompany> CreateCompanyAsync(CompanyCreationRequest company)
    {
        Company domainCompany = mapper.Map<Company>(company);
        _companies.CreateCompany(domainCompany);
        await repository.SaveAsync();
        return mapper.Map<ToClientCompany>(domainCompany);
    }

    /// <exception cref="IdParametersBadRequestException">Condition.</exception>
    /// <exception cref="CollectionByIdsBadRequestException">Condition.</exception>
    public async Task<ClientCompanies> GetCompaniesByIdsAsync(CompanyIds ids, bool trackChanges)
    {
        ThrowIfCompanyIdsAreNull();
        List<Guid> companyIdsList = ids.ToList();
        Companies domainCompanies = await GetCompaniesAsync();
        ThrowIfCompanyCountsMismatch();
        return mapper.Map<ClientCompanies>(domainCompanies);

        #region Nested_Helpers

        void ThrowIfCompanyIdsAreNull()
        {
            if (ids is null)
            {
                throw new IdParametersBadRequestException();
            }
        }
        
        async Task<Companies> GetCompaniesAsync() =>
            await _companies.GetCompaniesByTheirIdsAsync(companyIdsList, trackChanges);

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
    public async Task<(ClientCompanies clientCompanies, string ids)> CreateCompanyCollectionAsync(NewCompanies? newCompanies)
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
        await repository.SaveAsync();

        var clientCompanies = mapper.Map<ClientCompanies>(companies).ToList();
        var companyIds = string.Join(",", clientCompanies.Select((ToClientCompany c) => c.CompanyId));

        return (clientCompanies, companyIds);
    }

    public async Task DeleteCompanyByIdAsync(Guid companyId, bool trackChanges)
    {
        var domainCompany = await TryToGetCompanyAsync(companyId, trackChanges);
        _companies.DeleteCompany(domainCompany);
        await repository.SaveAsync();
    }

    public async Task UpdateCompanyAsync(Guid companyId, CompanyUpdateRequest companyUpdate, bool trackChanges)
    {
        var domainCompany = await TryToGetCompanyAsync(companyId, trackChanges);
        mapper.Map(companyUpdate, domainCompany);
        await repository.SaveAsync();
    }

    private async Task<Company> TryToGetCompanyAsync(Guid companyId, bool trackChanges) =>
        await _companies.GetCompanyAsync(companyId, trackChanges) ??
        throw new CompanyNotFoundException(companyId);
}