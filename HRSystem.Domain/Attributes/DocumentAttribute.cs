using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Domain.Attributes
{
    public class DocumentAttribute : AttributeBase, IAttributeWithValue<string>
    {
        public string Value { get; set; }
        
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