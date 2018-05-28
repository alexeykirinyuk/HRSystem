using System;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Domain.Attributes
{
    public class StringAttribute : AttributeBase, IAttributeWithValue<string>
    {
        public string Value { get; set; }

        public StringAttribute()
        {
        }

        public StringAttribute(Employee employee, AttributeInfo attributeInfo, string value) :
            base(employee, attributeInfo)
        {
            Value = value;
        }

        public override string GetValueAsString()
        {
            return Value ?? string.Empty;
        }

        public override void SetValueAsString(string value)
        {
            Value = value;
        }
    }
}