
using Application.Amana.Commands;
using Application.Amana.Queries;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmanaController : BaseController
    {
        [HttpPost("create")]
        public async Task<ActionResult> Create(CreateAmanaCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [HttpGet("search")]
        public async Task<ActionResult> GetWithPagination([FromQuery] GetAmanatWithPagination request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpPut("edit")]
        public async Task<ActionResult> Update(UpdateAmanaCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

      

        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> Remove([FromRoute] int id)
        {
            return ReturnResult(await Mediator.Send(new DeleteAmanaCommand { Id = id }));
        }
    }
}
