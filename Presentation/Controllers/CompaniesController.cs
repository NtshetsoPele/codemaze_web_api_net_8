namespace Presentation.Controllers;

[ApiController]
[Route(template: "api/[controller]")]
public class CompaniesController(IServiceManager service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllCompanies()
    {
        IEnumerable<ToClientCompany> companies = service.CompanyService.GetAllCompanies(trackChanges: false);

        return Ok(companies);
    }

    [HttpGet(template: "{companyId:guid}")]
    public IActionResult GetCompanyById(Guid companyId)
    {
        ToClientCompany company = service.CompanyService.GetCompanyById(companyId, trackChanges: false);

        if (company == null)
        {
            return NotFound();
        }

        return Ok(company);
    }
}