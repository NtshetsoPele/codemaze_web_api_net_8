namespace Repository;

public class CompanyRepository(RepositoryContext context) : 
    RepositoryBase<Company>(context), ICompanyRepository
{
    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
    {
        return FindAll(trackChanges).OrderBy((Company c) => c.Name).ToList();
    }
}