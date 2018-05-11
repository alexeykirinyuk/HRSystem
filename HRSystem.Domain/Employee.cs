using System;
using System.Collections.Generic;
using HRSystem.Domain.Attributes.Base;
using HRSystem.Global;
using HRSystem.Global.Errors;

namespace HRSystem.Domain
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