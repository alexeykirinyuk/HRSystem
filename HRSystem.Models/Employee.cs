using System;
using System.Collections.Generic;
using HRSystem.Common;
using HRSystem.Common.Errors;

namespace HRSystem.Models
{
    public class Employee
    {
        public string Login { get; set; }

        public virtual List<AttributeBase> Attributes { get; set; }

        [Obsolete(ErrorStrings.ForBindersOnly, true)]
        private Employee()
        {
        }

        public Employee(string login)
        {
            ArgumentValidator.EnsureNotNullOrEmpty(nameof(login), login);

            Login = login;
        }
    }
}