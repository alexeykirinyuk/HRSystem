using HRSystem.Domain;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Core
{
    public interface IDocumentService
    {
        void Save(Employee employee, AttributeInfo attributeInfo, Document document);

        Document Load(Employee employee, AttributeInfo attributeInfo);

        bool IsExists(Employee employee, AttributeInfo attributeInfo);

        void Delete(Employee employee, AttributeInfo attributeInfo);
    }
}