using HRSystem.Common.Errors;

namespace HRSystem.Models
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
    }
}