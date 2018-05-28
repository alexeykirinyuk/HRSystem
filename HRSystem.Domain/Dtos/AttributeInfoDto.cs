using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Web.Dtos
{
    public class AttributeInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AttributeType Type { get; set; }
        public ActiveDirectoryAttributeInfoDto ActiveDirectoryAttributeInfo { get; set; }
    }
}