using HRSystem.Common.Errors;

namespace HRSystem.Models.Attributes.Extensions
{
    public static class AttributeBaseIsExtensions
    {
        public static bool IsIntAttribute(this AttributeBase attributeBase)
        {
            ArgumentValidator.EnsureNotNull(nameof(attributeBase), attributeBase);
            
            return attributeBase is IntAttribute;
        }

        public static bool IsStringAttribute(this AttributeBase attributeBase)
        {
            ArgumentValidator.EnsureNotNull(nameof(attributeBase), attributeBase);

            return attributeBase is StringAttribute;
        }
    }
}