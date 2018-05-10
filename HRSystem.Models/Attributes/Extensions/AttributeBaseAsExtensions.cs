using HRSystem.Common.Errors;

namespace HRSystem.Models.Attributes.Extensions
{
    public static class AttributeBaseAsExtensions
    {
        public static IntAttribute AsIntAttribute(this AttributeBase attributeBase)
        {
            ArgumentValidator.EnsureNotNull(nameof(attributeBase), attributeBase);

            return (IntAttribute) attributeBase;
        }

        public static StringAttribute AsStringAttribute(this AttributeBase attributeBase)
        {
            ArgumentValidator.EnsureNotNull(nameof(attributeBase), attributeBase);

            return (StringAttribute) attributeBase;
        }
    }
}