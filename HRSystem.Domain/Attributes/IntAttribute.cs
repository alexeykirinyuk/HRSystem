using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Domain.Attributes
{
    public class IntAttribute : AttributeBase, IAttributeWithValue<int?>
    {
        public int? Value { get; set; }

        public IntAttribute()
        {
        }

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
                Value = int.Parse(value);
            }
        }
    }
}