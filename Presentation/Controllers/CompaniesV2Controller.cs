namespace Presentation.Controllers;

[ApiController, Route(template: "api/companies"), ApiVersion("2.0")]
public class CompaniesV2Controller(IServiceManager services) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCompanies()
    {
        return Ok(await GetCompaniesAsync());

        #region Nested_Helpers

        async Task<ClientCompanies> GetCompaniesAsync() =>
            await services.CompanyService.GetAllCompaniesAsync(trackChanges: false);

        #endregion
    }
}