using Presentation.ActionFilters;

namespace Presentation.Controllers;

[ApiController]
[Route(template: "api/[controller]")]
public class CompaniesController(IServiceManager service) : ControllerBase
{
    #region State

    private const string CompanyById = "CompanyById";
    private const string CompanyCollection = "CompanyCollection";
    private readonly ICompanyService _cmpService = service.CompanyService;

    #endregion

    [HttpGet]
    public async Task<IActionResult> GetAllCompanies()
    {
        return Ok(await GetCompaniesAsync());

        #region Nested_Helpers
        
        async Task<ClientCompanies> GetCompaniesAsync() =>
            await _cmpService.GetAllCompaniesAsync(trackChanges: false);

        #endregion
    }

    [HttpGet(template: "{companyId:guid}", Name = CompanyById)]
    public async Task<IActionResult> GetCompanyById([FromRoute] Guid companyId)
    {
        return Ok(await GetCompanyAsync());

        #region Nested_Helpers
        
        async Task<ToClientCompany> GetCompanyAsync() =>
            await _cmpService.GetCompanyByIdAsync(companyId, trackChanges: false);
        
        #endregion
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyCreationRequest? company)
    {
        if (company is null)
        {
            return BadRequest($"{nameof(company)} object is null");
        }

        var newCompany = await _cmpService.CreateCompanyAsync(company);
        
        return CreatedAtRoute(CompanyById, new { newCompany.CompanyId }, newCompany);
    }

    [HttpGet(template: "collection/({companyIds})", Name = CompanyCollection)]
    public async Task<IActionResult> GetCompanyCollection(
        [ModelBinder(binderType: typeof(ArrayModelBinder))] IEnumerable<Guid> companyIds)
    {
        return Ok(await GetCompaniesAsync());

        #region Nested_Helpers

        async Task<ClientCompanies> GetCompaniesAsync() =>
            await _cmpService.GetCompaniesByIdsAsync(ids: companyIds, trackChanges: false);

        #endregion
    }

    [HttpPost(template: "collection")]
    public async Task<IActionResult> CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyCreationRequest> newCompanies)
    {
        var (clientCompanies, ids) = await CreateCompaniesAsync();

        return CreatedAtRoute(CompanyCollection, new { companyIds = ids }, clientCompanies);

        #region Nested_Helpers

        async Task<(ClientCompanies clientCompanies, string ids)> CreateCompaniesAsync() =>
            await _cmpService.CreateCompanyCollectionAsync(newCompanies);

        #endregion
    }

    [HttpDelete(template: "{companyId:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid companyId)
    {
        await _cmpService.DeleteCompanyByIdAsync(companyId, trackChanges: false);
        
        return NoContent();
    }

    [HttpPut(template: "{companyId:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid companyId, [FromBody] CompanyUpdateRequest? companyUpdate)
    {
        if (companyUpdate is null)
        {
            return BadRequest($"{nameof(companyUpdate)} object is null");
        }
        
        await _cmpService.UpdateCompanyAsync(companyId, companyUpdate, trackChanges: true);

        return NoContent();
    }
}