using System.Security.Cryptography.X509Certificates;
using HRSystem.Common.Errors;

namespace HRSystem.Domain.Attributes.Base
{
    public abstract class AttributeBase : IAttribute
    {
        public int Id { get; set; }
        
        public string EmployeeLogin { get; set; }
        public Employee Employee { get; set; }
        
        public int AttributeInfoId { get; set; }
        public AttributeInfo AttributeInfo { get; set; }
        
        public AttributeType Descriminator { get; set; }
        
        protected AttributeBase()
        {
        }

        protected AttributeBase(Employee employee, AttributeInfo attributeInfo)
        {
            ArgumentHelper.EnsureNotNull("employee", employee);
            ArgumentHelper.EnsureNotNull("attributeInfo", attributeInfo);

            EmployeeLogin = employee.Login;
            Employee = employee;

            AttributeInfoId = attributeInfo.Id;
            AttributeInfo = attributeInfo;

            Descriminator = attributeInfo.Type;
        }

        public abstract string GetValueAsString();
        public abstract void SetValueAsString(string value);

        public IAttributeWithValue<T> GetValue<T>()
        {
            return (IAttributeWithValue<T>) this;
        }
    }
}