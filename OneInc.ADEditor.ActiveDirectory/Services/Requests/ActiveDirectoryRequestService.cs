using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using LiteGuard;
using OneInc.ADEditor.ActiveDirectory.Services.Connection.Interfaces;
using OneInc.ADEditor.ActiveDirectory.Services.Requests.Interfaces;

namespace OneInc.ADEditor.ActiveDirectory.Services.Requests
{
    internal class ActiveDirectoryRequestService : IActiveDirectoryRequestService
    {
        private readonly IActiveDirectoryConnectionService _connectionService;

        public ActiveDirectoryRequestService(IActiveDirectoryConnectionService connectionService)
        {
            Guard.AgainstNullArgument(nameof(connectionService), connectionService);

            _connectionService = connectionService;
        }

        public SearchResponse MakeSearchRequest(string path, string filter, SearchScope scope, params string[] attributes)
        {
            using (var connection = _connectionService.OpenConnection())
            {
                var request = new SearchRequest(path, filter, scope);
                request.Attributes.AddRange(attributes);

                return (SearchResponse)connection.SendRequest(request);
            }
        }

        public AddResponse MakeAddRequest(string id, DirectoryAttribute[] attributes)
        {
            using (var connection = _connectionService.OpenConnection())
            {
                var request = new AddRequest(id, attributes);

                return (AddResponse)connection.SendRequest(request);
            }
        }

        public ModifyResponse MakeModifyRequest(
            string id,
            DirectoryAttributeOperation operation,
            string attributeName,
            params object[] attributeValues)
        {
            using (var connection = _connectionService.OpenConnection())
            {
                var request = new ModifyRequest(id, operation, attributeName, attributeValues);

                return (ModifyResponse)connection.SendRequest(request);
            }
        }

        public ModifyResponse MakeModifyRequest(string id, IEnumerable<DirectoryAttributeModification> modifications)
        {
            using (var connection = _connectionService.OpenConnection())
            {
                var request = new ModifyRequest(id, modifications.ToArray());

                return (ModifyResponse)connection.SendRequest(request);
            }
        }

        public ModifyDNResponse MakeModifyDistinguishedName(string currentDistinguishedName, string parentDistinguishedName, string newName)
        {
            using (var connection = _connectionService.OpenConnection())
            {
                var request = new ModifyDNRequest(currentDistinguishedName, parentDistinguishedName, newName);

                return (ModifyDNResponse)connection.SendRequest(request);
            }
        }

        public DeleteResponse MakeDeleteRequest(string distinguishedName)
        {
            using (var connection = _connectionService.OpenConnection())
            {
                var request = new DeleteRequest(distinguishedName);

                return (DeleteResponse)connection.SendRequest(request);
            }
        }
    }
}
