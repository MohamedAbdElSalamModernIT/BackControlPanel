
using Application.Category.Commands;
using Application.Category.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers.Catalog
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        [HttpPost("create")]
        public async Task<ActionResult> Create(CreateCategoryCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }
        [HttpGet("search")]
        public async Task<ActionResult> GetWithPagination([FromQuery] GetCategoriesWithPaginationQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpPut("edit")]
        public async Task<ActionResult> Update(UpdateCategoryCommand request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpGet("find/{id}")]
        public async Task<ActionResult> GetWithId([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new GetCategoryWithIdQuery { Id = id }));
        }
        
        [HttpGet("suitable")]
        public async Task<ActionResult> GetWithParentId([FromQuery] GetSuitableListQuery request)
        {
            return ReturnResult(await Mediator.Send(request));
        }

        [HttpDelete("remove/{id}")]
        public async Task<ActionResult> Remove([FromRoute] string id)
        {
            return ReturnResult(await Mediator.Send(new DeleteCategoryCommand { Id = id }));
        }
    }
}
