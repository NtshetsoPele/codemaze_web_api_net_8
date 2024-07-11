namespace Repository;

public class CompanyRepository(RepositoryContext context) : RepositoryBase<Company>(context), ICompanyRepository;