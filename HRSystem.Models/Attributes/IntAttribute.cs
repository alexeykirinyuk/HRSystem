using System;
using HRSystem.Common;

namespace HRSystem.Models.Attributes
{
    public class IntAttribute : AttributeBase
    {
        public int? Value { get; set; }

        [Obsolete(ErrorStrings.ForBindersOnly, true)]
        private IntAttribute()
        {
        }

        public IntAttribute(string name, int? value) : base(name, AttributeType.Int)
        {
            Value = value;
        }
    }
}