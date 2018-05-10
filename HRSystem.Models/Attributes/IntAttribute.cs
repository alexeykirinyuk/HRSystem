namespace HRSystem.Models.Attributes
{
    public sealed class IntAttribute : AttributeBase
    {
        public int Value { get; set; }

        public IntAttribute()
        {
        }

        public IntAttribute(string name, int value) : base(name, AttributeType.Int)
        {
            Value = value;
        }
    }
}