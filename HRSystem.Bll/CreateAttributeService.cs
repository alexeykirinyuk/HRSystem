using System;
using HRSystem.Common.Errors;
using HRSystem.Core;
using HRSystem.Domain.Attributes;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Bll
{
    public class CreateAttributeService : ICreateAttributeService
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeService _attributeService;

        public CreateAttributeService(IEmployeeService employeeService, IAttributeService attributeService)
        {
            ArgumentHelper.EnsureNotNull(nameof(employeeService), employeeService);
            ArgumentHelper.EnsureNotNull(nameof(attributeService), attributeService);

            _employeeService = employeeService;
            _attributeService = attributeService;
        }

        public AttributeBase CreateAttribute(int attributeInfoId, string value, AttributeType type)
        {
            switch (type)
            {
                case AttributeType.Int:
                    return new IntAttribute
                    {
                        AttributeInfoId = attributeInfoId,
                        Descriminator = type,
                        Value = int.Parse(value)
                    };
                case AttributeType.String:
                    return new StringAttribute
                    {
                        AttributeInfoId = attributeInfoId,
                        Descriminator = type,
                        Value = value
                    };
                case AttributeType.DateTime:
                    return new DateTimeAttribute
                    {
                        AttributeInfoId = attributeInfoId,
                        Descriminator = type,
                        Value = DateTime.Parse(value)
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}