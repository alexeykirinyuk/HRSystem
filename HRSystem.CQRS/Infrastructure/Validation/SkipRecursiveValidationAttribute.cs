using System;

namespace HRSystem.CQRS.Infrastructure.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class SkipRecursiveValidationAttribute : Attribute
    {
    }
}