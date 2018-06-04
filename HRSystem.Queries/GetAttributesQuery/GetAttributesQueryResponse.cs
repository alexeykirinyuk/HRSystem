using System.Collections.Generic;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Queries.GetAttributesQuery
{
    public class GetAttributesQueryResponse
    {
        public ICollection<AttributeInfo> Attributes { get; set; }
    }
}