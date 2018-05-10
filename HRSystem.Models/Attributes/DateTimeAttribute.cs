using System;
using HRSystem.Common;

namespace HRSystem.Models.Attributes
{
    public class DateTimeAttribute : AttributeBase
    {
        public DateTime? Value { get; set; }
        
        [Obsolete(ErrorStrings.ForBindersOnly, true)]
        private DateTimeAttribute()
        {
        }

        public DateTimeAttribute(string name, DateTime? value) : base(name, AttributeType.DateTime)
        {
            Value = value;
        }
    }
}