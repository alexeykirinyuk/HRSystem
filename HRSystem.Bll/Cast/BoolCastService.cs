using HRSystem.Domain.Attributes;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Bll.Cast
{
    public class BoolCastService : ICastService
    {
        public AttributeType Type => AttributeType.Bool;
        
        public AttributeBase Cast(int attributeInfoId, string value)
        {
            return new BoolAttribute
            {
                AttributeInfoId = attributeInfoId,
                Descriminator = Type,
                Value = bool.Parse(value)
            };
        }
    }
}