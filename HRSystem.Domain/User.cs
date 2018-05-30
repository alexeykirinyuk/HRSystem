namespace HRSystem.Domain
{
    public class User
    {
        public string DistinguishedName { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string Phone { get; set; }
        public string JobTitle { get; set; }
        public string Office { get; set; }

        public string ManagerDistinguishedName { get; set; }
        public virtual User Manager { get; set; }
    }
}