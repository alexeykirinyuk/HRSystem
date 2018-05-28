using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Linq;

namespace OneInc.ADEditor.ActiveDirectory.Extensions
{
    public static class SearchResultEntryExtensions
    {
        public static string GetPropertyValue(this SearchResultEntry searchResultEntry, string attributeName)
        {
            var property = searchResultEntry.Attributes[attributeName];

            if ((property == null) || (property.Count != 1))
            {
                return string.Empty;
            }

            return (string)property.GetValues(typeof(string))[0];
        }

        public static IEnumerable<string> GetListPropertyValue(this SearchResultEntry searchResultEntry, string attributeName)
        {
            var property = searchResultEntry.Attributes[attributeName];

            return property?.GetValues(typeof(string)).Cast<string>() ?? Enumerable.Empty<string>();
        }

        public static DateTime GetDateTimePropertyValue(this SearchResultEntry searchResultEntry, string attributeName)
        {
            var stringValue = searchResultEntry.GetPropertyValue(attributeName);

            return DateTime.ParseExact(stringValue, ActiveDirectoryConstants.DateTimeFormat, CultureInfo.InvariantCulture);
        }
    }
}
