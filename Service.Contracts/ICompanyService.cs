namespace Service.Contracts;

public interface ICompanyService
{
    IEnumerable<ToClientCompany> GetAllCompanies(bool trackChanges);
}