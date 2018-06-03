using System;

namespace HRSystem.ActiveDirectory.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ConvertToActiveDirectoryString(this DateTime dateTime)
        {
            return dateTime.ToString(ActiveDirectoryConstants.DateTimeFormat);
        }
    }
}
