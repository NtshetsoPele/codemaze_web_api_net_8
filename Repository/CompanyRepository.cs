namespace Repository;

public class CompanyRepository(RepositoryContext context) : 
    RepositoryBase<Company>(context), ICompanyRepository
{
    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
    {
        return [.. FindAll(trackChanges).OrderBy((Company c) => c.Name)];
    }

    public Company? GetCompany(Guid companyId, bool trackChanges)
    {
        return FindByCondition(
                (Company c) => c.CompanyId.Equals(companyId),
                trackChanges)
            .SingleOrDefault();
    }

    public void CreateCompany(Company company) => Create(company);

    public IEnumerable<Company> GetCompaniesByTheirIds(IEnumerable<Guid> ids, bool trackChanges)
    {
        return [.. FindByCondition((Company c) => ids.Contains(c.CompanyId), trackChanges)];
    }

    public void DeleteCompany(Company company) => base.Delete(company);
}