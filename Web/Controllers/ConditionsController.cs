
using Application.Conditions.Commands;
using Application.Conditions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConditionsController : BaseController
    {

        [HttpGet("search")]
        public async Task<ActionResult> GetWithPagination([FromQuery] GetConditionsWithPaginationQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        
        [HttpGet("inactive")]
        public async Task<ActionResult> GetInActiveConditionsQuery([FromQuery] GetInActiveConditionsQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpGet("parameters")]
        public async Task<ActionResult> GetConditionsWithPatametrs([FromQuery] GetConditionsWithPatametrs request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpGet("conditionmap")]
        public async Task<ActionResult> GetConditionMap([FromQuery] GetConditionMapQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [HttpGet("all-conditions")]
        public async Task<ActionResult> GetConditions([FromQuery] GetConditions request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpPut("edit")]
        public async Task<ActionResult> Update(UpdateConditionCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCondition(AddConditionCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpDelete("remove/{alBaladiaID}/{buildingTypeID}/{conditionID}")]
        public async Task<ActionResult> DeleteCondition(
        int alBaladiaID,
        int buildingTypeID,
        double ConditionID
            )
        {
            return ReturnResult(await Mediator.Send(new DeleteConditionCommand()
            {
                AlBaladiaID = alBaladiaID,
                ConditionID = ConditionID,
                BuildingTypeID = buildingTypeID
            }));
        }
    }
}
