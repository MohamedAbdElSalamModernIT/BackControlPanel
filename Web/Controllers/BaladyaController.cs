
using Application.Baladya.Commands;
using Application.Baladya.Queries;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaladyaController : BaseController
    {
        [HttpPost("create")]
        public async Task<ActionResult> Create(CreateBaladyaCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [HttpGet("search")]
        public async Task<ActionResult> GetWithPagination([FromQuery] GetBaladyatWithPagination request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpPut("edit")]
        public async Task<ActionResult> Update(UpdateBaladyaCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

      

        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> Remove([FromRoute] int id)
        {
            return ReturnResult(await Mediator.Send(new DeleteBaladyaCommand { Id = id }));
        }
    }
}
