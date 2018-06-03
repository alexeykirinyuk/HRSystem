using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.Common.Errors;
using HRSystem.Common.Validation;
using HRSystem.Core;
using HRSystem.Global.Validation;

namespace HRSystem.Commands.SaveAttribute
{
    public class SaveAttributeCommandValidator : IValidator<SaveAttribute.SaveAttributeCommand>
    {
        private readonly IAttributeInfoService _attributeInfoService;

        public SaveAttributeCommandValidator(IAttributeInfoService attributeInfoService)
        {
            
            ArgumentHelper.EnsureNotNull(nameof(attributeInfoService), attributeInfoService);
            
            _attributeInfoService = attributeInfoService;
        }

        public async Task Validate(List<ValidationFailure> list, SaveAttributeCommand request)
        {
            list.NotNullOrEmpty("Name", request.Name);

            if (await _attributeInfoService.IsExists(request.Name).ConfigureAwait(false))
            {
                list.Add("Attribute with same name already exists.");
            }
            
            if (request.Id.HasValue)
            {
                if (!await _attributeInfoService.IsExists(request.Id.Value))
                {
                    list.Add("Attribute with same id wasn't found.");
                }
            }
        }
    }
}