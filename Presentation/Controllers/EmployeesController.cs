using Microsoft.AspNetCore.JsonPatch;
using Shared.ParameterObjects;

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
            new { companyId, clientEmployee.EmployeeId }, 
            clientEmployee);

        #region Nested_Helpers

        ToClientEmployee CreateNewEmployee() =>
            _empService.CreateCompanyEmployee(companyId, newEmployee, trackChanges: false);

        #endregion
    }

    [HttpDelete(template: "{employeeId:guid}")]
    public IActionResult DeleteCompanyEmployee(Guid companyId, Guid employeeId)
    {
        _empService.DeleteCompanyEmployee(companyId, employeeId, trackChanges: false);
        
        return NoContent();
    }

    [HttpPut(template: "{employeeId:guid}")]
    public IActionResult FullyUpdateCompanyEmployee(
        Guid companyId, Guid employeeId, [FromBody] EmployeeUpdateRequest? empUpdate)
    {
        if (empUpdate is null)
        {
            return BadRequest($"{nameof(empUpdate)} object is null");
        }
        
        _empService.UpdateCompanyEmployee(new CompanyEmployeeUpdateParameters
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
    public IActionResult PartiallyUpdateCompanyEmployee(
        Guid companyId, Guid employeeId, [FromBody] JsonPatchDocument<EmployeeUpdateRequest?>? patchDoc)
    {
        if (patchDoc is null)
        {
            return BadRequest("patchDoc is null.");
        }
        
        var (updateEmp, domainEmp) = _empService.GetPatchEmployee(
            new EmployeePatchParameters
            {
                CmpId = companyId,
                EmpId = employeeId,
                CmpTrackChanges = false,
                EmpTrackChanges = true
            });
        
        patchDoc.ApplyTo(updateEmp);
        
        _empService.ApplyPatch(updateEmp, domainEmp);

        return NoContent();
    }
}