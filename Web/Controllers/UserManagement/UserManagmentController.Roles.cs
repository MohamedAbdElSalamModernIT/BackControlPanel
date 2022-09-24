using System.Threading.Tasks;
using Application.Auth.Commands;
using Application.Auth.Queries;
using Application.UserManagment.Commands;
using Application.UserManagment.Queries;
using Domain.Enums.Roles;
using Microsoft.AspNetCore.Mvc;
using Web.CustomAttributes;

namespace Web.Controllers.UserManagement {
  public partial class UserManagementController {

    [HttpGet("roles")]
    [Permission(PermissionKeys.ViewRole)]
    public async Task<IActionResult> GetRoles(
      [FromQuery] GetRoleWithPagination query
    ) {
      return ReturnResult((await Mediator.Send(query)));
    }

    [HttpGet("roles/{id}")]
    public async Task<ActionResult> GetRole(
    [FromRoute] string id ){
        return ReturnResult((await Mediator.Send(new GetRoleWithId() { Id = id })));
    }

        [HttpPost("roles/add-role")]
    public async Task<ActionResult> AddRole([FromBody] AddRoleCommand command) {
      return ReturnResult((await Mediator.Send(command)));
    }

    [HttpPut("roles/edit-role/{id}")]
    public async Task<ActionResult> EditRole(
      [FromRoute] string id,
      [FromBody] EditRoleCommand command
    ) {
      command.Id = id;
      return ReturnResult((await Mediator.Send(command)));
    }
    

    [HttpDelete("roles/delete-role/{id}")]
    public async Task<ActionResult> DeleteRole([FromRoute] string id) {
      return ReturnResult(await Mediator.Send(new DeleteRoleCommand() {Id = id}));
    }

    [HttpGet("permissions")]
    public async Task<IActionResult> GetPermission() {
        return ReturnResult((await Mediator.Send(new GetAllPermissionQuery())));
    }

    [HttpPost("add-permission")]
    public async Task<IActionResult> AddPermissionToRole([FromBody] AddPermissionToRolCommand command) {
        return ReturnResult((await Mediator.Send(command)));
    }


    }
}