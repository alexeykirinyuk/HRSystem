using System;

namespace HRSystem.Models.Attributes
{
    public sealed class DateTimeAttribute : AttributeBase
    {
        public DateTime? Value { get; set; }
        
        public DateTimeAttribute()
        {
        }

        public DateTimeAttribute(string name, DateTime? value) : base(name, AttributeType.DateTime)
        {
            Value = value;
        }
    }
}