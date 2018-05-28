using HRSystem.Domain.Attributes;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Bll.Cast
{
    public class IntCastService : ICastService
    {
        public AttributeType Type => AttributeType.Int;
        
        public AttributeBase Cast(int attributeInfoId, string value)
        {
            return new IntAttribute
            {
                AttributeInfoId = attributeInfoId,
                Descriminator = Type,
                Value = int.Parse(value)
            };
        }
    }
}