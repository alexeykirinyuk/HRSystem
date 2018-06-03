using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Domain.Attributes
{
    public class DocumentAttribute : AttributeBase, IAttributeWithValue<bool>
    {
        public bool Value { get; set; }

        public DocumentAttribute(bool hasFile, Employee employee, AttributeInfo attributeInfo) : base(employee, attributeInfo)
        {
            Value = hasFile;
        }
        
        public override string GetValueAsString()
        {
            return Value.ToString().ToLower();
        }

        public override void SetValueAsString(string value)
        {
            Value = bool.Parse(value);
        }
    }
}