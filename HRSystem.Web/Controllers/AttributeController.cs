using System.Threading.Tasks;
using HRSystem.Commands.DeleteAttribute;
using HRSystem.Commands.SaveAttribute;
using HRSystem.Common.Errors;
using HRSystem.Common.Validation;
using HRSystem.Queries.GetAttributesQuery;
using HRSystem.Queries.GetAttributeSavingInfo;
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
        public Task<GetAttributesQueryResponse> GetAll()
        {
            return _mediator.Send(new GetAttributesQuery());
        }

        [HttpGet("savingInfo/{id?}")]
        public Task<GetAttributeSavingInfoQueryResponse> GetSavingInfo(int? id)
        {
            return _mediator.Send(new GetAttributeSavingInfoQuery {Id = id});
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
                return BadRequest(e.ToResponse());
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteAttributeCommand() {AttributeInfoId = id}).ConfigureAwait(false);
                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.ToResponse());
            }
        }
    }
}