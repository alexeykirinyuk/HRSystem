using System;
using HRSystem.Common;

namespace HRSystem.Models.Attributes
{
    public class StringAttribute : AttributeBase
    {
        public string Value { get; set; }

        [Obsolete(ErrorStrings.ForBindersOnly, true)]
        private StringAttribute()
        {
        }

        public StringAttribute(string name, string value) : base(name, AttributeType.String)
        {
            Value = value;
        }
    }
}