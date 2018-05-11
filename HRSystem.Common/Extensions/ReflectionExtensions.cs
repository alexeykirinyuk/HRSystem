using System.Reflection;
using HRSystem.Global.Errors;

namespace HRSystem.Global.Extensions
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