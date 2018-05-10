using System.Reflection;
using HRSystem.Common.Errors;

namespace HRSystem.Common.Extensions
{
    public static class ReflectionExtensions
    {
        public static object GetPropertyValue(this object obj, PropertyInfo propertyInfo)
        {
            ArgumentValidator.EnsureNotNull(nameof(obj), obj);
            ArgumentValidator.EnsureNotNull(nameof(propertyInfo), propertyInfo);

            return propertyInfo.GetValue(obj);
        }
    }
}