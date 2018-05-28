using System.Collections.Generic;
using HRSystem.Global.Validation;

namespace HRSystem.Common.Validation
{
    public static class ValidatorExtension
    {
        public static void NotNullOrEmpty(this List<ValidationFailure> list, string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                list.Add(new ValidationFailure($"{name} can't be null or empty."));
            }
        }

        public static void Add(this List<ValidationFailure> list, string message)
        {
            list.Add(new ValidationFailure(message));
        }
    }
}