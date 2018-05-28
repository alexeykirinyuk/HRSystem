using System;
using HRSystem.Domain.Attributes;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Bll.Cast
{
    public class DateTimeCastService : ICastService
    {
        public AttributeType Type => AttributeType.DateTime;
        
        public AttributeBase Cast(int attributeInfoId, string value)
        {
            return new DateTimeAttribute
            {
                AttributeInfoId = attributeInfoId,
                Descriminator = Type,
                Value = DateTime.Parse(value)
            };
        }
    }
}