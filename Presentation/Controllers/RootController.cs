namespace Presentation.Controllers;

[Route(template: "api"), ApiController]
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
            )
        };

        return Ok(list);
    }
}