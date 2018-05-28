using System.Collections.Generic;

namespace HRSystem.Web.Dtos
{
    public class EmployeeDto
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string JobTitle { get; set; }
        public string Office { get; set; }
        
        public EmployeeDto Manager { get; set; }
        public ICollection<EmployeeAttributeDto> Attributes { get; set; }
    }
}