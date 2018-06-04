using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Common.Validation;
using HRSystem.Core;
using HRSystem.Global.Validation;

namespace HRSystem.Queries.GetAttributeSavingInfo
{
    public class GetAttributeSavingInfoQueryValidator : IValidator<GetAttributeSavingInfoQuery>
    {
        private readonly IAttributeInfoService _attributeInfoService;

        public GetAttributeSavingInfoQueryValidator(IAttributeInfoService attributeInfoService)
        {
            ArgumentHelper.EnsureNotNull(nameof(attributeInfoService), attributeInfoService);

            _attributeInfoService = attributeInfoService;
        }

        public async Task Validate(List<ValidationFailure> list, GetAttributeSavingInfoQuery request)
        {
            if (!request.Id.HasValue)
            {
                return;
            }

            if (!await _attributeInfoService.IsExists(request.Id.Value))
            {
                list.Add("Attribute with same id wasn't found.");
            }
        }
    }
}