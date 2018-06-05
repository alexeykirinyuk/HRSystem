using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRSystem.Commands.SaveEmployee;
using HRSystem.Common.Errors;
using HRSystem.Common.Validation;
using HRSystem.Queries.GetEmployees;
using HRSystem.Queries.GetEmployeeSavingInfo;
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
        public Task<GetEmployeesQueryResponse> GetEmployees()
        {
            var queryParameters = Request.Query.ToDictionary(q => q.Key, q => q.Value);
            
            return _mediator.Send(new GetEmployeesQuery
            {
                ManagerFullNameFilter = queryParameters.GetValueOrDefault("manager"),
                OfficeFilter = queryParameters.GetValueOrDefault("office"),
                JobTitleFilter = queryParameters.GetValueOrDefault("jobTitle"),
                AllAttributesFilter = queryParameters.GetValueOrDefault("allAttributes"),
                AttributeFilters = queryParameters.Where(q => q.Key != "manager" &&
                                                              q.Key != "office" &&
                                                              q.Key != "jobTitle" &&
                                                              q.Key != "allAttributes")
                    .ToDictionary(q => int.Parse(q.Key), q => q.Value.ToString())
            });
        }

        [HttpGet("creationInfo")]
        public Task<GetEmployeeSavingInfoQueryResponse> GetCreateEmploeeInfo(string login, bool isCreate)
        {
            return _mediator.Send(new GetEmployeeSavingInfoQuery
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