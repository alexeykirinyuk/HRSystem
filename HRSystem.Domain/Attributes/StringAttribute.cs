using System;
using HRSystem.Domain.Attributes.Base;
using HRSystem.Global;

namespace HRSystem.Domain.Attributes
{
    public class StringAttribute : AttributeWithValue<string>
    {
        [Obsolete(ErrorStrings.ForBindersOnly, true)]
        private StringAttribute()
        {
        }

        public StringAttribute(string name, string value) : base(name, value, AttributeType.String)
        {
        }
    }
}