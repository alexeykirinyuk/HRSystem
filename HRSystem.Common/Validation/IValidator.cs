using System.Collections.Generic;
using System.Threading.Tasks;
using HRSystem.Global.Validation;

namespace HRSystem.Common.Validation
{
    public interface IValidator<in TRequest>
    {
        Task Validate(List<ValidationFailure> list, TRequest request);
    }
}