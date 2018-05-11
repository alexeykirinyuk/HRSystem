using System;
using HRSystem.Domain.Attributes.Base;
using HRSystem.Global;

namespace HRSystem.Domain.Attributes
{
    public class IntAttribute : AttributeWithValue<int?>
    {
        [Obsolete(ErrorStrings.ForBindersOnly, true)]
        private IntAttribute()
        {
        }

        public IntAttribute(string name, int? value) : base(name, value, AttributeType.Int)
        {
        }
    }
}