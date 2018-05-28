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

        public AttributeInfo(
            string name,
            AttributeType type,
            ActiveDirectoryAttributeInfo activeDirectoryAttributeInfo = default)
        {
            ArgumentHelper.EnsureNotNullOrEmpty("name", name);
            
            Name = name;
            Type = type;

            ActiveDirectoryAttributeInfoId = activeDirectoryAttributeInfo?.Id;
            ActiveDirectoryAttributeInfo = activeDirectoryAttributeInfo;
        }
    }
}