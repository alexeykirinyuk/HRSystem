namespace OneInc.ADEditor.Dal
{
    public static class SharePointEntitiesConstants
    {
        public static class ProductAttributes
        {
            public const string Id = "ID";
            public const string Name = "Title";
        }

        public static class EmployeeAttributes
        {
            public const string Id = "ID";

            public const string Photo = "Photo";
            public const string Name = "Title";
            public const string User = "User";
            public const string Office = "Office";
            public const string Manager = "Manager";
            public const string Department = "Department";
            public const string Job = "Job";

            public const string PhotoSmall = "PhotoSmall";
            public const string PhotoLarge = "PhotoLarge";

            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string AccountName = "AccountName";
            public const string Email = "Email";

            public const string ActiveDirectorySynced = "ADSynced";
            public const string SharePointSynced = "SPSynced";
            public const string AdminApproved = "AdminApproved";
            public const string Errors = "ADEditorErrors";

            public const string Phone = "Phone";
            public const string Fired = "Fired";

            public const string WhenUpdated = "Modified";
            public const string CreatedBy = "Author";
            public const string ModifiedBy = "Editor";
        }

        public static class OfficeAttributes
        {
            public const string Location = "Title";
        }

        public class JobAttributes
        {
            public const string Id = "ID";
            public const string Title = "Title";
        }
    }
}