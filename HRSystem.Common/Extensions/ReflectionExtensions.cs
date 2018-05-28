using System.Reflection;
using HRSystem.Common.Errors;

namespace HRSystem.Global.Extensions
{
    public static class ReflectionExtensions
    {
        public static object GetPropertyValue(this object obj, PropertyInfo propertyInfo)
        {
            ArgumentHelper.EnsureNotNull(nameof(obj), obj);
            ArgumentHelper.EnsureNotNull(nameof(propertyInfo), propertyInfo);

            return propertyInfo.GetValue(obj);
        }
    }
}