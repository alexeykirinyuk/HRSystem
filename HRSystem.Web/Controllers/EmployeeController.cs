using System.Threading.Tasks;
using HRSystem.Commands.SaveEmployee;
using HRSystem.Common.Errors;
using HRSystem.Common.Validation;
using HRSystem.Queries.EmployeeQuery;
using HRSystem.Queries.EmployeeSavingInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Web.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            ArgumentHelper.EnsureNotNull(nameof(mediator), mediator);

            _mediator = mediator;
        }

        [HttpGet("all")]
        public Task<EmployeeQueryResponse> GetEmployees()
        {
            return _mediator.Send(new EmployeeQuery());
        }

        [HttpGet("creationInfo")]
        public Task<EmployeeSavingInfoQueryResponse> GetCreateEmploeeInfo(string login, bool isCreate)
        {
            return _mediator.Send(new EmployeeSavingInfoQuery
            {
                Login = login,
                IsCreate = isCreate
            });
        }

        [HttpPost("save")]
        public async Task<ActionResult> Save([FromBody] SaveEmployeeCommand request)
        {
            try
            {
                await _mediator.Send(request).ConfigureAwait(false);
                return Ok();
            }
            catch (ValidationException validationException)
            {
                return BadRequest(validationException.ToResponse());
            }
        }
    }
}