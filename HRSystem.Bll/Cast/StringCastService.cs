using HRSystem.Domain.Attributes;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Bll.Cast
{
    public class StringCastService : ICastService
    {
        public AttributeType Type => AttributeType.String;

        public AttributeBase Cast(int attributeInfoId, string value)
        {
            return new StringAttribute
            {
                AttributeInfoId = attributeInfoId,
                Descriminator = Type,
                Value = value
            };
        }
    }
}