using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HRSystem.Global.Errors;
using HRSystem.Global.Extensions;

namespace HRSystem.Infrastructure.Infrastructure.Validation
{
    public static class DataAnnotationsValidator
    {
        //The code is based on https://github.com/reustmd/DataAnnotationsValidatorRecursive
        public static bool TryValidateObject(object obj, ICollection<ValidationResult> results, IServiceProvider serviceProvider)
        {
            return Validator.TryValidateObject(obj, new ValidationContext(obj, serviceProvider, null), results, true);
        }

        public static bool TryValidateObjectRecursive<T>(T obj, List<ValidationResult> results, IServiceProvider serviceProvider)
        {
            ArgumentValidator.EnsureNotNull(nameof(obj), obj);
            ArgumentValidator.EnsureNotNull(nameof(results), results);
            ArgumentValidator.EnsureNotNull(nameof(serviceProvider), serviceProvider);
            
            var result = TryValidateObject(obj, results, serviceProvider);

            var properties =
                obj.GetType()
                    .GetProperties()
                    .Where(prop => prop.CanRead && !prop.GetCustomAttributes(typeof(SkipRecursiveValidationAttribute), false).Any())
                    .ToList();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(string) || property.PropertyType.GetTypeInfo().IsValueType)
                {
                    continue;
                }

                var value = obj.GetPropertyValue(property);
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (value == null)
                {
                    continue;
                }

                if (value is IEnumerable asEnumerable)
                {
                    result = asEnumerable
                        .Cast<object>()
                        .Aggregate(result, (current, enumObj) => current & TryValidateChildObjectRecursive(enumObj, property, results, serviceProvider));
                }
                else
                {
                    result &= TryValidateChildObjectRecursive(value, property, results, serviceProvider);
                }
            }

            return result;
        }

        private static bool TryValidateChildObjectRecursive(
            object value,
            PropertyInfo parentProperty,
            ICollection<ValidationResult> runningValidationResults,
            IServiceProvider serviceProvider)
        {
            var nestedResults = new List<ValidationResult>();
            if (TryValidateObjectRecursive(value, nestedResults, serviceProvider))
            {
                return true;
            }

            foreach (var validationResult in nestedResults)
            {
                runningValidationResults.Add(
                    new ValidationResult(
                        validationResult.ErrorMessage,
                        validationResult.MemberNames.Select(x => parentProperty.Name + '.' + x)));
            }

            return false;
        }
    }

}