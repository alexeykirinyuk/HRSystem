using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Queries.GetAttributeSavingInfo
{
    public class GetAttributeSavingInfoQueryResponse
    {
        public string Name { get; set; }
        public AttributeType Type { get; set; }
    }
}