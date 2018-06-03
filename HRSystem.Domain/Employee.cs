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

        public void Update(string firstName,
            string lastName,
            string email,
            string phone,
            string jobTitle,
            string office,
            string managerLogin,
            List<AttributeBase> attributes)
        {
            var withoutChanges =
                FirstName == firstName &&
                LastName == lastName &&
                Email == email &&
                JobTitle == jobTitle &&
                Office == office &&
                ManagerLogin == managerLogin;
            if (!withoutChanges)
            {
                LastModified = DateTime.UtcNow;
            }
            
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            JobTitle = jobTitle;
            Office = office;
            ManagerLogin = managerLogin;

            foreach (var attr in attributes)
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
                attributes.All(a => a.AttributeInfoId != currentAttribute.AttributeInfoId));
        }

        public User ToUser(string managerDistinguishedName)
        {
            return new User
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

        public bool ContainsText(string searchFilter)
        {
            var result = false;
            result |= ContainsIfNotNull(FullName, searchFilter);
            result |= ContainsIfNotNull(JobTitle, searchFilter);
            result |= ContainsIfNotNull(Login, searchFilter);
            result |= ContainsIfNotNull(Email, searchFilter);
            result |= ContainsIfNotNull(Office, searchFilter);
            result |= ContainsIfNotNull(Manager?.FullName, searchFilter);

            return result;
        }

        private bool ContainsIfNotNull(string property, string searchFilter)
        {
            return property != null && property.Contains(searchFilter);
        }
    }
}