
using Application.Category.Commands;
using Application.Category.Queries;
using Application.Drawings.Commands;
using Application.Drawings.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
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
        public async Task<ActionResult> GetDrawingsWithPagination([FromQuery] GetDrawingsWithPaginationQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [HttpGet("drawings-logs")]
        public async Task<ActionResult> GetDrawingLogsWithPagination([FromQuery] GetDrawingLogsWithPaginationQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpGet("client-drawings")]
        public async Task<ActionResult> GetDrawingsQuery([FromQuery] GetDrawingsQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpGet("excel/{id}")]
        [AllowAnonymous]
        public async Task<FileContentResult> ExportToExcel([FromRoute] string id)
        {
            var payload = await Mediator.Send(new GetLogsReaultsQuery { LogId = id });

            byte[] buffer = (byte[])payload.Payload;

            return File(buffer, "application/octet-stream", "ConditionsResult.xlsx");
        }
        
        [HttpGet("file/{id}")]
        [AllowAnonymous]
        public async Task<FileContentResult> GetDrawingFile([FromRoute] string id)
        {
            var payload = await Mediator.Send(new GetDrawingFileQuery { DrawingId = id });

            var file = (AppFile)payload.Payload;

            return File(file.Bytes, "application/octet-stream", file.Name);
        }
    }
}
