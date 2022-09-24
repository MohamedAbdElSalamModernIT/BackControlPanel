
using Application.Category.Commands;
using Application.Category.Queries;
using Application.Lookup.Commands;
using Application.Lookup.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : BaseController
    {

        [HttpGet("information")]
        public async Task<ActionResult> GetWithPagination([FromQuery] GetInformationQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [HttpPut("information")]
        public async Task<ActionResult> UpdateInformationCommand(UpdateInformationCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpGet("types")]
        public async Task<ActionResult> GetBuildingTypesAndConditionsQuery([FromQuery] GetBuildingTypesQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [HttpGet("versions")]
        public async Task<ActionResult> GetVersionsQuery([FromQuery] GetVersionsQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [HttpGet("amanat")]
        public async Task<ActionResult> GetAmanat([FromQuery] GetAmanatByAreaIdQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpGet("baladyat/{id}")]
        public async Task<ActionResult> GetBaladyat([FromRoute] int id)
        {
            return ReturnResult(await Mediator.Send(new GetBaladyatByAmanaIdQuery { AmanaId = id }));
        }

    }
}
