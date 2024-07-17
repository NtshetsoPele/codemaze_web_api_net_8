namespace Service.Contracts;

public interface ICompanyService
{
    ClientCompanies GetAllCompanies(bool trackChanges);
    ToClientCompany GetCompanyById(Guid id, bool trackChanges);
    ToClientCompany CreateCompany(CompanyCreationRequest company);
    ClientCompanies GetCompaniesByIds(CompanyIds ids, bool trackChanges);
    (ClientCompanies clientCompanies, string ids) CreateCompanyCollection(NewCompanies newCompanies);
    void DeleteCompanyById(Guid companyId, bool trackChanges);
}