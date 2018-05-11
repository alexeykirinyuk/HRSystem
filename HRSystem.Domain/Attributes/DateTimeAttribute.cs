using System;
using HRSystem.Domain.Attributes.Base;
using HRSystem.Global;
using HRSystem.Global.Errors;

namespace HRSystem.Domain.Attributes
{
    public class DateTimeAttribute : AttributeWithValue<DateTime?>
    {
        [Obsolete(ErrorStrings.ForBindersOnly, true)]
        private DateTimeAttribute()
        {
        }

        public DateTimeAttribute(string name, DateTime? value) : base(name, value, AttributeType.DateTime)
        {
        }
    }
}