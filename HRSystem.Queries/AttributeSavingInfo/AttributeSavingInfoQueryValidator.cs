using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Common.Validation;
using HRSystem.Core;
using HRSystem.Global.Validation;

namespace HRSystem.Queries.AttributeSavingInfo
{
    public class AttributeSavingInfoQueryValidator : IValidator<AttributeSavingInfoQuery>
    {
        private readonly IAttributeService _attributeService;

        public AttributeSavingInfoQueryValidator(IAttributeService attributeService)
        {
            ArgumentHelper.EnsureNotNull(nameof(attributeService), attributeService);

            _attributeService = attributeService;
        }

        public async Task Validate(List<ValidationFailure> list, AttributeSavingInfoQuery request)
        {
            if (!request.Id.HasValue)
            {
                return;
            }

            if (!await _attributeService.IsExists(request.Id.Value))
            {
                list.Add("Attribute with same id wasn't found.");
            }
        }
    }
}