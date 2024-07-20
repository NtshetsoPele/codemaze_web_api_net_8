namespace Presentation.Controllers;

[ApiController]
[Route(template: "api/companies/{companyId:guid}/[controller]")]
public class EmployeesController(IServiceManager service) : ControllerBase
{
    #region State

    private readonly IEmployeeService _empService = service.EmployeeService;
    private const string EmployeeById = "EmployeeById";

    #endregion

    [HttpGet]
    public IActionResult GetCompanyEmployees([FromRoute] Guid companyId)
    {
        return Ok(GetEmployees());

        #region Nested_Helpers

        ClientEmployees GetEmployees() =>
            _empService.GetCompanyEmployees(companyId, trackChanges: false);

        #endregion
    }

    [HttpGet(template: "{employeeId:guid}", Name = EmployeeById)]
    public IActionResult GetCompanyEmployee(
        [FromRoute] Guid companyId, [FromRoute] Guid employeeId)
    {
        return Ok(GetEmployee());

        #region Nested_Helpers

        ToClientEmployee GetEmployee() =>
            _empService.GetCompanyEmployee(companyId, employeeId, trackChanges: false);

        #endregion
    }

    [HttpPost]
    public IActionResult CreateEmployee(
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
        
        var clientEmployee = CreateNewEmployee();
        
        return CreatedAtRoute(
            EmployeeById, 
            routeValues: new { companyId, employeeId = clientEmployee.EmployeeId }, 
            clientEmployee);

        #region Nested_Helpers

        ToClientEmployee CreateNewEmployee() =>
            _empService.CreateCompanyEmployee(companyId, newEmployee, trackChanges: false);

        #endregion
    }
}