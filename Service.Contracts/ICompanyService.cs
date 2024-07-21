namespace Service.Contracts;

public interface ICompanyService
{
    Task<ClientCompanies> GetAllCompaniesAsync(bool trackChanges);
    Task<ToClientCompany> GetCompanyByIdAsync(Guid id, bool trackChanges);
    Task<ToClientCompany> CreateCompanyAsync(CompanyCreationRequest company);
    Task<ClientCompanies> GetCompaniesByIdsAsync(CompanyIds ids, bool trackChanges);
    Task<(ClientCompanies clientCompanies, string ids)> CreateCompanyCollectionAsync(NewCompanies newCompanies);
    Task DeleteCompanyByIdAsync(Guid companyId, bool trackChanges);
    Task UpdateCompanyAsync(Guid companyId, CompanyUpdateRequest companyUpdate, bool trackChanges);
}