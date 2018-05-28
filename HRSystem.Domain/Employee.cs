using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HRSystem.Common.Errors;
using HRSystem.Domain.Attributes.Base;
    
namespace HRSystem.Domain
{
    public class Employee
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string JobTitle { get; set; }
        
        public string ManagerLogin { get; set; }
        public virtual Employee Manager { get; set; }

        public virtual List<AttributeBase> Attributes { get; set; }

        public Employee()
        {
        }

        public Employee(string login)
        {
            ArgumentHelper.EnsureNotNullOrEmpty(nameof(login), login);

            Login = login;
        }

        public void AddAttribute(int attributeInfoId, AttributeType type, string value)
        {
            
        }
    }
}