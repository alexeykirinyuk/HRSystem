using System;
using System.Collections.Generic;
using HRSystem.Global.Validation;

namespace HRSystem.Common.Validation
{
    public class ValidationException : Exception
    {
        public ICollection<ValidationFailure> Failures { get; }

        public ValidationException(ICollection<ValidationFailure> failures)
        {
            Failures = failures;
        }

        public ValidationException(string failure)
        {
            Failures = new List<ValidationFailure> {failure};
        }
    }
}