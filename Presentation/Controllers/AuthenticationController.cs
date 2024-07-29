namespace Presentation.Controllers;

[ApiController]
[Route(template: "api/[controller]")]
public class AuthenticationController(IServiceManager services) : ControllerBase
{
    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest userRegistration)
    {
        IdentityResult result = await services.AuthenticationService.RegisterUser(userRegistration);
        if (result.Succeeded)
        {
            return StatusCode(201);
        }
        foreach (IdentityError error in result.Errors)
        {
            ModelState.TryAddModelError(key: error.Code, error.Description);
        }
        return BadRequest(ModelState);
    }
}