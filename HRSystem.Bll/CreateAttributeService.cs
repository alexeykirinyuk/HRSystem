using System;
using System.Collections.Generic;
using System.Linq;
using HRSystem.Bll.Cast;
using HRSystem.Core;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Bll
{
    public class CreateAttributeService : ICreateAttributeService
    {
        private readonly ICollection<ICastService> _castServices;

        public CreateAttributeService(IEnumerable<ICastService> castServices)
        {
            _castServices = castServices.ToArray();
        }

        public AttributeBase CreateAttribute(int attributeInfoId, string value, AttributeType type)
        {
            return _castServices.Single(service => service.Type == type)
                .Cast(attributeInfoId, value);
        }
    }
}