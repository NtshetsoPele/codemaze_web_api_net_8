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
        [FromRoute] Guid companyId, [FromBody] EmployeeCreationRequest newEmployee)
    {
        var clientEmployee = CreateNewEmployee();
        
        return CreatedAtRoute(
            EmployeeById, 
            routeValues: new { companyId, clientEmployee.EmployeeId }, 
            clientEmployee);

        #region Nested_Helpers

        ToClientEmployee CreateNewEmployee() =>
            _empService.CreateCompanyEmployee(companyId, newEmployee, trackChanges: false);

        #endregion
    }
}