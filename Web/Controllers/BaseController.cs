using Common;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Controllers
{
    [Authorization]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();


        protected ObjectResult ReturnResult(Result result)
        {
            switch (result.Code)
            {
                case ApiExceptionType.Ok:
                    return Ok(result);
                case ApiExceptionType.BadRequest:
                    return BadRequest(result);
                case ApiExceptionType.Unauthorized:
                    return Unauthorized(result);
                case ApiExceptionType.NotFound:
                    return NotFound(result);
                default:
                    return BadRequest(result);
            }
        }
    }
}