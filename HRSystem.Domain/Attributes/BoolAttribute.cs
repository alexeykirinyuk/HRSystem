using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Domain.Attributes
{
    public class BoolAttribute : AttributeBase, IAttributeWithValue<bool?>
    {
        public bool? Value { get; set; }
        
        public override string GetValueAsString()
        {
            return Value?.ToString() ?? string.Empty;
        }

        public override void SetValueAsString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Value = null;
            }
            else
            {
                Value = bool.Parse(value);
            }
        }
    }
}