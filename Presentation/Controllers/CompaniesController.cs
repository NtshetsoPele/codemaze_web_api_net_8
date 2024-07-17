using Presentation.ModelBinders;

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
    public IActionResult GetAllCompanies()
    {
        return Ok(GetCompanies());

        #region Nested_Helpers
        
        ClientCompanies GetCompanies() =>
            _cmpService.GetAllCompanies(trackChanges: false);

        #endregion
    }

    [HttpGet(template: "{companyId:guid}", Name = CompanyById)]
    public IActionResult GetCompanyById([FromRoute] Guid companyId)
    {
        return Ok(GetCompany());

        #region Nested_Helpers
        
        ToClientCompany GetCompany() =>
            _cmpService.GetCompanyById(companyId, trackChanges: false);
        
        #endregion
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyCreationRequest? company)
    {
        if (company is null)
        {
            return BadRequest($"{nameof(company)} object is null");
        }

        var newCompany = _cmpService.CreateCompany(company);
        
        return CreatedAtRoute(CompanyById, new { newCompany.CompanyId }, newCompany);
    }

    [HttpGet(template: "collection/({companyIds})", Name = CompanyCollection)]
    public IActionResult GetCompanyCollection(
        [ModelBinder(binderType: typeof(ArrayModelBinder))] IEnumerable<Guid> companyIds)
    {
        return Ok(value: GetCompanies());

        #region Nested_Helpers

        ClientCompanies GetCompanies() =>
            _cmpService.GetCompaniesByIds(ids: companyIds, trackChanges: false);

        #endregion
    }

    [HttpPost(template: "collection")]
    public IActionResult CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyCreationRequest> newCompanies)
    {
        var (clientCompanies, ids) = CreateCompanies();

        return CreatedAtRoute(CompanyCollection, new { companyIds = ids }, clientCompanies);

        #region Nested_Helpers

        (ClientCompanies clientCompanies, string ids) CreateCompanies() =>
            _cmpService.CreateCompanyCollection(newCompanies);

        #endregion
    }

    [HttpDelete]
    public IActionResult DeleteCompany(Guid companyId)
    {
        _cmpService.DeleteCompanyById(companyId, trackChanges: false);
        
        return NoContent();
    }
}