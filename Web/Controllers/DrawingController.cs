
using Application.Category.Commands;
using Application.Category.Queries;
using Application.Drawings.Commands;
using Application.Drawings.Dto;
using Application.Drawings.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Web.CustomAttributes;

namespace Web.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawingController : BaseController
    {
        [HttpPost("create-drawing")]
        public async Task<ActionResult> Create(CreateDrawingAdminCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        //[HttpPost("admin-create-drawing")]
        //public async Task<ActionResult> Create(CreateDrawingAdminCommand request)
        //{
        //    return ReturnResult(await Mediator.Send(request));
        //}

        [HttpPost("create-Log")]

        public async Task<ActionResult> CreateLog(CreateDrawingLogCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpPost("upload-file")]
        [DisableRequestSizeLimit]
        [DisableFormValueModelBinding]
        public async Task<ActionResult> Uploadfile([FromForm] UploadfileCommand request)
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

        [HttpGet("{id}")]

        public async Task<ActionResult> GetDrawingWithId([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new GetDrawingWithIdQuery { Id = id }));
        }

        [HttpGet("details/{id}")]

        public async Task<ActionResult> GetDrawingDetailsWithId([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new GetDrawingDetailsWithIdQuery { Id = id }));
        }

        [HttpPut("edit-drawing")]
        public async Task<ActionResult> EditDrawing(EditDrawingCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpPut("edit-status")]
        public async Task<ActionResult> EditDrawingStatus(EditDrawingStatusCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpPut("finish-drawing")]

        public async Task<ActionResult> FinishDrawing(FinishDrawingCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteDrawing([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new DeleteDrawingCommand { Id = id }));
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
