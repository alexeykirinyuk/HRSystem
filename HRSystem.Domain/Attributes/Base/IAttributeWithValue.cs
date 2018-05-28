namespace HRSystem.Domain.Attributes.Base
{
    public interface IAttributeWithValue<TValue> : IAttribute
    {
        TValue Value { get; set; }
    }
}