using System.Collections.Generic;

namespace HRSystem.Models
{
    public sealed class Employee
    {
        public int Id { get; set; }
        
        public string Login { get; set; }

        public ICollection<AttributeBase> Attributes;
    }
}