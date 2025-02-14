﻿namespace Repository;

public class CompanyRepository(RepositoryContext context) : RepositoryBase<Company>(context), ICompanyRepository
{
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges)
    {
        return await 
            FindAll(trackChanges)
                .OrderBy((Company c) => c.Name)
                .ToListAsync();
    }

    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<Company?> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        return await 
            FindByCondition((Company c) => c.CompanyId.Equals(companyId), trackChanges)
                .SingleOrDefaultAsync();
    }

    public void CreateCompany(Company company) => Create(company);

    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    public async Task<IEnumerable<Company>> GetCompaniesByTheirIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        return await 
            FindByCondition((Company c) => ids.Contains(c.CompanyId), trackChanges)
                .ToListAsync();
    }

    public void DeleteCompany(Company company) => Delete(company);
}