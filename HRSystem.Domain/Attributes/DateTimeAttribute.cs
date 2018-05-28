using System;
using HRSystem.Domain.Attributes.Base;
using HRSystem.Global;

namespace HRSystem.Domain.Attributes
{
    public class DateTimeAttribute : AttributeBase, IAttributeWithValue<DateTime?>
    {
        public DateTime? Value { get; set; }

        public DateTimeAttribute()
        {
        }

        public DateTimeAttribute(Employee employee, AttributeInfo attributeInfo, DateTime? value) : base(employee, attributeInfo)
        {
            Value = value;
        }
    }
}