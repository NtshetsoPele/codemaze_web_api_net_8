namespace Presentation.Controllers;

[ApiController]
[Route(template: "api/companies/{companyId:guid}/[controller]")]
public class EmployeesController(IServiceManager service) : ControllerBase
{
    #region State

    private const string EmployeeById = "EmployeeById";
    private readonly IEmployeeService _empService = service.EmployeeService;

    #endregion

    [HttpGet]
    [HttpHead]
    public async Task<IActionResult> GetCompanyEmployees(
        [FromRoute] Guid companyId, [FromQuery] EmployeeParameters parameters)
    {
        (IEnumerable<ExpandoObject> employees, MetaData metaData) = await GetEmployeesAsync();

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));
        
        return Ok(employees);

        #region Nested_Helpers

        async Task<(IEnumerable<ExpandoObject>, MetaData)> GetEmployeesAsync() =>
            await _empService.GetCompanyEmployeesAsync(companyId, parameters, trackChanges: false);

        #endregion
    }

    [HttpGet(template: "{employeeId:guid}", Name = EmployeeById)]
    public async Task<IActionResult> GetCompanyEmployee(
        [FromRoute] Guid companyId, [FromRoute] Guid employeeId)
    {
        return Ok(await GetEmployeeAsync());

        #region Nested_Helpers

        async Task<ToClientEmployee> GetEmployeeAsync() =>
            await _empService.GetCompanyEmployeeAsync(companyId, employeeId, trackChanges: false);

        #endregion
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee(
        [FromRoute] Guid companyId, [FromBody] EmployeeCreationRequest? newEmployee)
    {
        if (newEmployee is null)
        {
            return BadRequest("New employee details not found.");
        }

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        
        var clientEmployee = await CreateNewEmployeeAsync();
        
        return CreatedAtRoute(
            EmployeeById, 
            new { companyId, clientEmployee.EmployeeId }, 
            clientEmployee);

        #region Nested_Helpers

        async Task<ToClientEmployee> CreateNewEmployeeAsync() =>
            await _empService.CreateCompanyEmployeeAsync(companyId, newEmployee, trackChanges: false);

        #endregion
    }

    [HttpDelete(template: "{employeeId:guid}")]
    public async Task<IActionResult> DeleteCompanyEmployee(Guid companyId, Guid employeeId)
    {
        await _empService.DeleteCompanyEmployeeAsync(companyId, employeeId, trackChanges: false);
        
        return NoContent();
    }

    [HttpPut(template: "{employeeId:guid}")]
    public async Task<IActionResult> FullyUpdateCompanyEmployee(
        Guid companyId, Guid employeeId, [FromBody] EmployeeUpdateRequest? empUpdate)
    {
        if (empUpdate is null)
        {
            return BadRequest($"{nameof(empUpdate)} object is null");
        }
        
        await _empService.UpdateCompanyEmployeeAsync(new CompanyEmployeeUpdateParameters
        {
            CmpId = companyId,
            EmpId = employeeId,
            EmpUpdate = empUpdate,
            CmpTrackChanges = false,
            EmpTrackChanges = true
        });

        return NoContent();
    }

    [HttpPatch(template: "{employeeId:guid}")]
    public async Task<IActionResult> PartiallyUpdateCompanyEmployee(
        Guid companyId, Guid employeeId, [FromBody] JsonPatchDocument<EmployeeUpdateRequest?>? patchDoc)
    {
        if (patchDoc is null)
        {
            return BadRequest("Patch request is empty.");
        }
        
        var (updateEmp, domainEmp) = await _empService.GetPatchEmployeeAsync(
            new EmployeePatchParameters
            {
                CmpId = companyId,
                EmpId = employeeId,
                CmpTrackChanges = false,
                EmpTrackChanges = true
            });
        
        patchDoc.ApplyTo(updateEmp, ModelState);

        TryValidateModel(updateEmp);

        if (!ModelState.IsValid)
        {
            return UnprocessableEntity("Patch details failed validation.");
        }
        
        await _empService.ApplyPatchAsync(updateEmp, domainEmp);

        return NoContent();
    }
}