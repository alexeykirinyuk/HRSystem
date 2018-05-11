using System;

namespace HRSystem.Infrastructure.Infrastructure.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class SkipRecursiveValidationAttribute : Attribute
    {
    }
}