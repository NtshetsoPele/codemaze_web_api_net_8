namespace Service.Contracts;

public interface ICompanyService
{
    IEnumerable<ToClientCompany> GetAllCompanies(bool trackChanges);
    ToClientCompany GetCompanyById(Guid companyId, bool trackChanges);
    ToClientCompany CreateCompany(CompanyCreationRequest company);
}