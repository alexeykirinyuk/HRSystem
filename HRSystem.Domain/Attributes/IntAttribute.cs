using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Domain.Attributes
{
    public class IntAttribute : AttributeBase, IAttributeWithValue<int?>
    {
        public int? Value { get; set; }
        
        public IntAttribute()
        {
        }

        public IntAttribute(Employee employee, AttributeInfo attributeInfo, int? value) : 
            base(employee, attributeInfo)
        {
            Value = value;
        }
    }
}