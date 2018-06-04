using System;
using HRSystem.Common.Errors;
using HRSystem.Global;

namespace HRSystem.Domain.Attributes.Base
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AttributeInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AttributeType Type { get; set; }

        public bool IsActiveDirectoryAttribute => ActiveDirectoryAttributeInfoId.HasValue;

        public int? ActiveDirectoryAttributeInfoId { get; set; }

        public ActiveDirectoryAttributeInfo ActiveDirectoryAttributeInfo { get; set; }

        public AttributeInfo()
        {
        }

        public void Update(AttributeInfo attributeInfo)
        {
            Name = attributeInfo.Name;
            Type = attributeInfo.Type;
        }
    }
}