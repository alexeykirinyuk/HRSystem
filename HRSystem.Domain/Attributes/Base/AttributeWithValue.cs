namespace HRSystem.Domain.Attributes.Base
{
    public class AttributeWithValue<T>: AttributeBase
    {
        public T Value;

        public AttributeWithValue() : base()
        {
        }
        
        public AttributeWithValue(string name, T value, AttributeType type) : base(name, type)
        {
            Value = value;
        }
    }
}