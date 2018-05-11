using HRSystem.Global.Errors;

namespace HRSystem.Domain.Attributes.Base
{
    public abstract class AttributeBase
    {
        public int Id { get; set; }
        
        public string EmployeeLogin { get; set; }
        
        public Employee Employee { get; set; }

        public string Name { get; set; }

        public AttributeType Type { get; set; }
        
        public bool IsActiveDirectoryAttribute { get; set; }

        public AttributeBase()
        {
        }

        public AttributeBase(string name, AttributeType type)
        {
            ArgumentValidator.EnsureNotNullOrEmpty(nameof(name), name);

            Name = name;
            Type = type;
        }

        public AttributeWithValue<T> WithData<T>()
        {
            return (AttributeWithValue<T>) this;
        }
    }
}