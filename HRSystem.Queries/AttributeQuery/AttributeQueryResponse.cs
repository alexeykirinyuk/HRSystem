using System.Collections.Generic;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Queries.AttributeQuery
{
    public class AttributeQueryResponse
    {
        public ICollection<AttributeInfo> Attributes { get; set; }
    }
}