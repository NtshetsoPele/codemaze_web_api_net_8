namespace Presentation.Controllers;

[ApiController]
[Route(template: "api/companies/{companyId:guid}/[controller]")]
public class EmployeesController(IServiceManager service) : ControllerBase
{
    private readonly IEmployeeService _empService = service.EmployeeService;
    private const string EmployeeById = "EmployeeById";

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
        //var clientEmployee = CreateNewEmployee();

        var clientEmployee = new ToClientEmployee
        {
            EmployeeId = Guid.NewGuid(),
            Name = newEmployee.Name, 
            Age = newEmployee.Age, 
            Position = newEmployee.Position
        };
        
        return CreatedAtAction(
            EmployeeById, 
            routeValues: new { companyId, employeeId = clientEmployee.EmployeeId }, 
            clientEmployee);

        #region Nested_Helpers

        ToClientEmployee CreateNewEmployee() =>
            _empService.CreateCompanyEmployee(companyId, newEmployee, trackChanges: false);

        #endregion
    }
}