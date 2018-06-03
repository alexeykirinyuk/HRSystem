using System;
using System.Collections.Generic;
using System.Linq;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Domain
{
    public class Employee
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string Phone { get; set; }
        public string JobTitle { get; set; }
        public string Office { get; set; }
        public DateTime LastModified { get; set; }

        public string ManagerLogin { get; set; }
        public virtual Employee Manager { get; set; }

        public virtual List<AttributeBase> Attributes { get; set; }

        public void Update(Employee employee)
        {
            var withoutChanges =
                FirstName == employee.FirstName &&
                LastName == employee.LastName &&
                Email == employee.Email &&
                JobTitle == employee.JobTitle &&
                Office == employee.Office &&
                ManagerLogin == employee.ManagerLogin;
            
            if (!withoutChanges)
            {
                LastModified = DateTime.UtcNow;
            }
            
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Email = employee.Email;
            Phone = employee.Phone;
            JobTitle = employee.JobTitle;
            Office = employee.Office;
            ManagerLogin = employee.ManagerLogin;

            foreach (var attr in employee.Attributes)
            {
                var singleAttribute = Attributes.SingleOrDefault(currentAttribute =>
                    currentAttribute.AttributeInfoId == attr.AttributeInfoId);

                if (singleAttribute == null)
                {
                    Attributes.Add(attr);
                }
                else
                {
                    singleAttribute.SetValueAsString(attr.GetValueAsString());
                }
            }

            Attributes.RemoveAll(currentAttribute =>
                employee.Attributes.All(a => a.AttributeInfoId != currentAttribute.AttributeInfoId));
        }

        public Account ToUser(string managerDistinguishedName)
        {
            return new Account
            {
                Email = Email,
                FirstName = FirstName,
                JobTitle = JobTitle,
                LastName = LastName,
                Login = Login,
                ManagerDistinguishedName = managerDistinguishedName,
                Office = Office,
                Phone = Phone
            };
        }
    }
}