using ClientEmployees = System.Collections.Generic.IEnumerable<Shared.DTOs.Responses.ToClientEmployee>;

namespace Presentation.Controllers;

[ApiController]
[Route(template: "api/companies/{companyId:guid}/[controller]")]
public class EmployeesController(IServiceManager service) : ControllerBase
{
    private readonly IEmployeeService _empService = service.EmployeeService;
    
    [HttpGet]
    public IActionResult GetCompanyEmployees(Guid companyId)
    {
        return Ok(GetEmployees());

        ClientEmployees GetEmployees() =>
            _empService.GetCompanyEmployees(companyId, trackChanges: false);
    }

    [HttpGet(template: "{employeeId:guid}")]
    public IActionResult GetCompanyEmployee(Guid companyId, Guid employeeId)
    {
        return Ok(GetEmployee());

        ToClientEmployee GetEmployee() =>
            _empService.GetCompanyEmployee(companyId, employeeId, trackChanges: false);
    }
}