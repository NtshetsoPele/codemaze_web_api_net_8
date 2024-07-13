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
}