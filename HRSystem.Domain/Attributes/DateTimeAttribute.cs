using System;
using System.Globalization;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Domain.Attributes
{
    public class DateTimeAttribute : AttributeBase, IAttributeWithValue<DateTime?>
    {
        private const string DateTimeFormat = "yyyy-MM-dd";
        
        public DateTime? Value { get; set; }

        public DateTimeAttribute()
        {
        }

        public DateTimeAttribute(Employee employee, AttributeInfo attributeInfo, DateTime? value) : base(employee,
            attributeInfo)
        {
            Value = value;
        }

        public override string GetValueAsString()
        {
            return Value?.ToString(DateTimeFormat) ?? string.Empty;
        }

        public override void SetValueAsString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Value = null;
            }
            else
            {
                Value = DateTime.ParseExact(value, DateTimeFormat, CultureInfo.InvariantCulture);
            }
        }
    }
}