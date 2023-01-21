
using Application.Office.Commands;
using Application.Office.Queries;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeController : BaseController
    {
        [HttpPost("create")]
        public async Task<ActionResult> Create(CreateOfficeCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetWithId([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new GetOfficeWithIdQuery { Id = id }));
        }

        [HttpGet("search")]
        public async Task<ActionResult> GetWithPagination([FromQuery] GetAllOfficesWithPagination request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpPut("edit")]
        public async Task<ActionResult> Update(UpdateOfficeCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> Remove([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new DeleteOfficeCommand { Id = id }));
        }
    }
}
