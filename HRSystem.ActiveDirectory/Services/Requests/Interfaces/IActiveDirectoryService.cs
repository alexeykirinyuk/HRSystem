using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace HRSystem.ActiveDirectory.Services.Requests.Interfaces
{
    public interface IActiveDirectoryService
    {
        IEnumerable<SearchResultEntry> FindEntities(
            string path,
            string filter,
            SearchScope scope = SearchScope.Subtree,
            params string[] attributes);

        SearchResultEntry Find(string path, string filter, SearchScope scope = SearchScope.Subtree);

        string Create(string path, string name, DirectoryAttribute[] attributes);

        void Update(string id, DirectoryAttributeOperation operation, string attributeName, params string[] attributeValues);

        void UpdateSafe(
            string distinguishedName,
            DirectoryAttributeOperation operation,
            string attributeName,
            params string[] attributeValues);

        void Update(string id, IEnumerable<DirectoryAttributeModification> modifications);

        string UpdateDistinguishedName(string oldDistinguishedName, string parentDistinguishedName, string name);

        void Delete(string distinguishedName);
    }
}
