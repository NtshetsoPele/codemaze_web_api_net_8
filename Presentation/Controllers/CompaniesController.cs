namespace Presentation.Controllers;

[ApiController]
[Route(template: "api/[controller]")]
public class CompaniesController(IServiceManager service) : ControllerBase
{
    #region State

    private const string CompanyById = "CompanyById";
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
    public IActionResult CreateCompany([FromBody] CompanyCreationRequest company)
    {
        var newCompany = _cmpService.CreateCompany(company);
        
        return CreatedAtRoute(CompanyById, routeValues: new { newCompany.CompanyId }, newCompany);
    }
}