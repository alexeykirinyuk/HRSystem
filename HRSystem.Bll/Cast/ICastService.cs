using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Bll.Cast
{
    public interface ICastService
    {
        AttributeType Type { get; }
        
        AttributeBase Cast(int attributeInfoId, string value);
    }
}