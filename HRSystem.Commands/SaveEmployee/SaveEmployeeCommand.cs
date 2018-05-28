using System.Collections.Generic;
using HRSystem.Domain.Attributes.Base;
using MediatR;

namespace HRSystem.Commands.SaveEmployee
{
    public sealed class EmployeeAttribute
    {
        public int AttributeInfoId { get; set; }
        public AttributeType Type { get; set; }
        public string Value { get; set; }
    }
    
    public sealed class SaveEmployeeCommand : IRequest
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public string Phone { get; set; }
        public string ManagerLogin { get; set; }
        public ICollection<EmployeeAttribute> Attributes { get; set; }
        public bool Create { get; set; }
    }
}