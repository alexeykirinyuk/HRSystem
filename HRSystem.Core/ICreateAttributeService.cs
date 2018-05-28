using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Core
{
    public interface ICreateAttributeService
    {
        AttributeBase CreateAttribute(int attributeInfoId, string value, AttributeType type);
    }
}