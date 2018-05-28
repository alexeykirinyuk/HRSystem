using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Domain;
using HRSystem.Domain.Attributes.Base;
using HRSystem.Web.Dtos;
using MediatR;

namespace HRSystem.Queries.EmployeeSavingInfo
{
    public class
        EmployeeSavingInfoQueryHandler : IRequestHandler<EmployeeSavingInfoQuery, EmployeeSavingInfoQueryResponse>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeService _attributeService;

        public EmployeeSavingInfoQueryHandler(IEmployeeService employeeService, IAttributeService attributeService)
        {
            ArgumentHelper.EnsureNotNull(nameof(employeeService), employeeService);
            ArgumentHelper.EnsureNotNull(nameof(attributeService), attributeService);

            _employeeService = employeeService;
            _attributeService = attributeService;
        }

        public async Task<EmployeeSavingInfoQueryResponse> Handle(EmployeeSavingInfoQuery request,
            CancellationToken cancellationToken)
        {
            var employee = await GetEmployee(request.IsCreate, request.Login);
            var attributes = await _attributeService.GetAll().ConfigureAwait(false);
            var employees = await _employeeService.GetAll().ConfigureAwait(false);

            return new EmployeeSavingInfoQueryResponse
            {
                Employee = Mapper.Map<EmployeeDto>(employee),
                Attributes =  Mapper.Map<ICollection<AttributeInfoDto>>(attributes.ToArray()),
                Employees =  Mapper.Map<ICollection<EmployeeDto>>(employees.ToArray())
            };
        }

        public Task<Employee> GetEmployee(bool isCreate, string login)
        {
            if (!isCreate)
            {
                return _employeeService.GetByLogin(login);
            }

            return Task.FromResult(new Employee
            {
                Login = string.Empty,
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                ManagerLogin = null,
                Manager = null,
                Attributes = new List<AttributeBase>(),
                JobTitle = string.Empty,
                Office = string.Empty,
                Phone = string.Empty
            });
        }
    }
}