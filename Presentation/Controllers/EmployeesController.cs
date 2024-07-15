namespace Presentation.Controllers;

[ApiController]
[Route(template: "api/companies/{companyId:guid}/[controller]")]
public class EmployeesController(IServiceManager service) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCompanyEmployees(Guid companyId)
    {
        IEnumerable<ToClientEmployee> employees = 
            service.EmployeeService.GetCompanyEmployees(companyId, trackChanges: false);
        
        return Ok(employees);
    }

    [HttpGet(template: "{employeeId:guid}")]
    public IActionResult GetCompanyEmployee(Guid companyId, Guid employeeId)
    {
        ToClientEmployee employee =
            service.EmployeeService.GetCompanyEmployee(companyId, employeeId, trackChanges: false);

        return Ok(employee);
    }
}