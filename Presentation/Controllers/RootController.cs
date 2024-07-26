namespace Presentation.Controllers;

[Route(template: "api"), ApiController, ApiVersion(version: "1.0")]
public class RootController(LinkGenerator linkGenerator) : ControllerBase
{
    [HttpGet(Name = "GetRoot")]
    public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
    {
        if (!mediaType.Contains("application/vnd.my-vendor.apiroot"))
        {
            return NoContent();
        }
        
        var list = new List<Link>
        {
            new(
                href: linkGenerator.GetUriByName(HttpContext, nameof(GetRoot), new {})!,
                rel: "self",
                method: "GET"
            ),
            new(
                href: linkGenerator.GetUriByName(HttpContext, "GetCompanies", new {})!,
                rel: "companies",
                method: "GET"
            ),
            new(
                href: linkGenerator.GetUriByName(HttpContext, "CreateCompany", new {})!,
                rel: "create_company",
                method: "POST"
            )
        };

        return Ok(list);
    }
}