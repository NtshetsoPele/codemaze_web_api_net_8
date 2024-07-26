namespace Presentation.Controllers;

[ApiController, Route(template: "api/companies")/*, ApiVersion("2.0", Deprecated = true)*/]
public class CompaniesV2Controller(IServiceManager services) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        return Ok((await GetCompaniesAsync()).Select((ToClientCompany c) => $"{c.Name} --> V2"));

        #region Nested_Helpers

        async Task<ClientCompanies> GetCompaniesAsync() =>
            await services.CompanyService.GetAllCompaniesAsync(trackChanges: false);

        #endregion
    }
}