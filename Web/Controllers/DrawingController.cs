
using Application.Category.Commands;
using Application.Category.Queries;
using Application.Drawings.Commands;
using Application.Drawings.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawingController : BaseController
    {
        [HttpPost("create-drawing")]
        public async Task<ActionResult> Create(CreateDrawingCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
       
        [HttpPost("create-Log")]
        public async Task<ActionResult> CreateLog(CreateDrawingLogCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpGet("drawings")]
        public async Task<ActionResult> GetConditionsWithPatametrs([FromQuery] GetDrawingsWithPaginationQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        
        [HttpGet("client-drawings")]
        public async Task<ActionResult> GetDrawingsQuery([FromQuery] GetDrawingsQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
    }
}
