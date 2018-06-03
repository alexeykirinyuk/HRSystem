using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.Common.Validation;
using HRSystem.Core;
using HRSystem.Global.Validation;

namespace HRSystem.Commands.DeleteAttribute
{
    public class DeleteAttributeCommandValidator : IValidator<DeleteAttributeCommand>
    {
        private readonly IAttributeInfoService _attributeInfoService;

        public DeleteAttributeCommandValidator(IAttributeInfoService attributeInfoService)
        {
            _attributeInfoService = attributeInfoService;
        }

        public async Task Validate(List<ValidationFailure> list, DeleteAttributeCommand request)
        {
            if (!await _attributeInfoService.IsExists(request.AttributeInfoId))
            {
                list.Add("Attribute not found.");
            }
        }
    }
}