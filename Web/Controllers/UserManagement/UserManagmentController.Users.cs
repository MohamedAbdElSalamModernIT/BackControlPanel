using System.Threading.Tasks;
using Application.UserManagment.Commands;
using Application.UserManagment.Queries;
using Domain.Enums.Roles;
using Microsoft.AspNetCore.Mvc;
using Web.CustomAttributes;

namespace Web.Controllers.UserManagement
{
    public partial class UserManagementController
    {

        [HttpGet("users")]
        //[Permission(PermissionKeys.ReadUser)]
        public async Task<IActionResult> Get([FromQuery] GetUserListQuery query)
        {
            return ReturnResult((await Mediator.Send(query)));
        }
        [HttpGet("users/{id}")]
        //[Permission(PermissionKeys.ReadUser)]
        public async Task<IActionResult> GetById([FromRoute] GetUserByIdQuery query)
        {
            return ReturnResult((await Mediator.Send(query)));
        }

        [HttpPost("users")]
        //[Permission(PermissionKeys.AddUser)]
        public async Task<ActionResult> AddUser([FromBody] AddUserCommand command)
        {
            return ReturnResult((await Mediator.Send(command)));
        }


        [HttpPut("users/edit-user/{id}")]
        //[Permission(PermissionKeys.EditUser)]
        public async Task<ActionResult> EditUser(
            [FromRoute] string id,
            [FromBody] EditUserCommand command
          )
        {
            command.Id = id;
            return ReturnResult((await Mediator.Send(command)));
        }
        [HttpDelete("users/delete-user/{id}")]
        //[Permission(PermissionKeys.RemoveUser)]
        public async Task<ActionResult> DeleteUser([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new DeleteUserCommand() { Id = id }));
        }


    }
}