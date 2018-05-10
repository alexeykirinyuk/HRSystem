using HRSystem.Common.Errors;

namespace HRSystem.Models
{
    public abstract class AttributeBase
    {
        public int Id { get; set; }
        
        public int EmployeeId { get; set; }

        public string Name { get; set; }

        public AttributeType AttributeType { get; set; }

        public AttributeBase()
        {
        }

        public AttributeBase(string name, AttributeType attributeType)
        {
            ArgumentValidator.EnsureNotNullOrEmpty(nameof(name), name);

            Name = name;
            AttributeType = attributeType;
        }
    }
}