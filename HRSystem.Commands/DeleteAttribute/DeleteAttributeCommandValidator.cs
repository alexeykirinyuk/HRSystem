using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.Common.Validation;
using HRSystem.Core;
using HRSystem.Global.Validation;

namespace HRSystem.Commands.DeleteAttribute
{
    public class DeleteAttributeCommandValidator : IValidator<DeleteAttributeCommand>
    {
        private readonly IAttributeService _attributeService;

        public DeleteAttributeCommandValidator(IAttributeService attributeService)
        {
            _attributeService = attributeService;
        }

        public async Task Validate(List<ValidationFailure> list, DeleteAttributeCommand request)
        {
            if (!await _attributeService.IsExists(request.AttributeInfoId))
            {
                list.Add("Attribute not found.");
            }
        }
    }
}