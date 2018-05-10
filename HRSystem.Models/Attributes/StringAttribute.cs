namespace HRSystem.Models.Attributes
{
    public sealed class StringAttribute : AttributeBase
    {
        public string Value { get; set; }

        public StringAttribute()
        {
        }

        public StringAttribute(string name, string value) : base(name, AttributeType.String)
        {
            Value = value;
        }
    }
}