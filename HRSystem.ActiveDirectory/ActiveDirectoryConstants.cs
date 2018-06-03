namespace HRSystem.ActiveDirectory
{
    public static class ActiveDirectoryConstants
    {
        public static class Entities
        {
            public const string User = "User";
            public const string OrganizationalUnit = "OrganizationalUnit";
            public const string Office = "Office";
        }

        public static class EntityAttributes
        {
            public const string DistinguishedName = "DistinguishedName";
            public const string Type = "ObjectClass";

            public const string Name = "Name";
            public const string DisplayName = "DisplayName";
            public const string AccountName = "SamAccountName";
            public const string Email = "Mail";
            public const string FirstName = "GivenName";
            public const string LastName = "sn";
            public const string Office = "PhysicalDeliveryOfficeName";
            public const string MemberOf = "MemberOf";
            public const string Manager = "Manager";
            public const string Department = "Department";
            public const string Job = "Title";
            public const string Phone = "TelephoneNumber";
            public const string Password = "UnicodePwd";
            public const string WhenUpdated = "WhenChanged";
            public const string Location = "L";

            public const string UserAccountControl = "UserAccountControl";
            public const string UserAccountControlValue = "544";
            public const string UserAccountControlBlocked = "userAccountControl:1.2.840.113556.1.4.803:";
            public const string UserAccountControlBlockedValue = "2";

            public const string UserPrincipalName = "UserPrincipalName";
            public const string UserMustChangePassword = "PwdLastSet";
            public const string UserNotMustChangePasswordValue = "-1";
        }

        public static class Paths
        {
            public const string UserOfficePrefix = "OU=Users";
        }

        public const string DateTimeFormat = "yyyyMMddHHmmss.0Z";
    }
}
