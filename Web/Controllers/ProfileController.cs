
using Application.Office.Commands;
using Application.Office.Queries;
using Application.UserManagment.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : BaseController
    {

        [HttpGet("engineer/{id}")]
        public async Task<ActionResult> GetWithId([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new GetEngineerProfileQuery { Id = id }));
        }
        
        [HttpGet("office/{id}")]
        public async Task<ActionResult> GetOfficeProfile([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new GetOfficeProfileQuery { Id = id }));
        }


    }
}
