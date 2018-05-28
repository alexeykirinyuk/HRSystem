using System;
using System.Threading.Tasks;
using HRSystem.Commands.SaveAttribute;
using HRSystem.Common.Errors;
using HRSystem.Common.Validation;
using HRSystem.Queries.AttributeQuery;
using HRSystem.Queries.AttributeSavingInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Web.Controllers
{
    [Route("api/[controller]")]
    public class AttributeController : Controller
    {
        private readonly IMediator _mediator;

        public AttributeController(IMediator mediator)
        {
            ArgumentHelper.EnsureNotNull(nameof(mediator), mediator);

            _mediator = mediator;
        }

        [HttpGet("all")]
        public Task<AttributeQueryResponse> GetAll()
        {
            return _mediator.Send(new AttributeQuery());
        }

        [HttpGet("savingInfo/{id?}")]
        public Task<AttributeSavingInfoQueryResponse> GetSavingInfo(int? id)
        {
            return _mediator.Send(new AttributeSavingInfoQuery {Id = id});
        }

        [HttpPost("save")]
        public async Task<ActionResult> Save([FromBody] SaveAttributeCommand request)
        {
            try
            {
                await _mediator.Send(request).ConfigureAwait(false);
                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Failures);
            }
        }
    }
}