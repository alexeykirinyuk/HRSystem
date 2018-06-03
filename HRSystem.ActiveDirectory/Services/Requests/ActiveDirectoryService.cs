using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using HRSystem.ActiveDirectory.Services.Requests.Interfaces;
using LiteGuard;
using NLog;

namespace HRSystem.ActiveDirectory.Services.Requests
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly IActiveDirectoryRequestService _requestService;
        private readonly IActiveDirectoryDistinguishedNameBuilderService _distinguishedNameBuilderService;
        private readonly Logger _logger;

        public ActiveDirectoryService(
            IActiveDirectoryRequestService requestService,
            IActiveDirectoryDistinguishedNameBuilderService distinguishedNameBuilderService)
        {
            Guard.AgainstNullArgument(nameof(requestService), requestService);
            Guard.AgainstNullArgument(nameof(distinguishedNameBuilderService), distinguishedNameBuilderService);

            _requestService = requestService;
            _distinguishedNameBuilderService = distinguishedNameBuilderService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public IEnumerable<SearchResultEntry> FindEntities(
            string path,
            string filter,
            SearchScope scope = SearchScope.Subtree,
            params string[] attributes)
        {
            SearchResponse response;
            try
            {
                response = _requestService.MakeSearchRequest(path, filter, scope, attributes);
            }
            catch (LdapException exception)
            {
                _logger.Error(
                    exception,
                    $"Error when finding entities. Path: {path}. Filter: {filter}. Scope: {scope}. Attributes: {string.Join(",", attributes)}");

                throw;
            }

            return response.Entries.Cast<SearchResultEntry>();
        }

        public SearchResultEntry Find(string path, string filter, SearchScope scope = SearchScope.Subtree)
        {
            var entities = Enumerable.Empty<SearchResultEntry>().ToArray();

            try
            {
                entities = FindEntities(path, filter, scope).ToArray();
            }
            catch (DirectoryOperationException exception)
            {
                _logger.Warn(exception, $"Exception when finding entity. Path: {path}. Filter: {filter}. Scope: {scope}");

                if (exception.Response.ResultCode != ResultCode.NoSuchObject)
                {
                    throw;
                }
            }

            if (entities.Length > 1)
            {
                _logger.Warn($"There are more than one element for path: '${path}; filter: '${filter}'");
            }

            return entities.FirstOrDefault();
        }

        public string Create(string path, string name, DirectoryAttribute[] attributes)
        {
            var distinguishedName = _distinguishedNameBuilderService.BuildDistinguishedName(name, path);
            _requestService.MakeAddRequest(distinguishedName, attributes);

            return distinguishedName;
        }

        public void Update(string id, DirectoryAttributeOperation operation, string attributeName, params string[] attributeValues)
        {
            _requestService.MakeModifyRequest(id, operation, attributeName, attributeValues.Cast<object>().ToArray());
        }

        public void UpdateSafe(
            string distinguishedName,
            DirectoryAttributeOperation operation,
            string attributeName,
            params string[] attributeValues)
        {
            try
            {
                Update(distinguishedName, operation, attributeName, attributeValues);
            }
            catch (DirectoryOperationException exception)
            {
                var resultCode = exception.Response.ResultCode;
                _logger.Warn(exception, $"Error while updating active directory entity, result code: '{resultCode}'");

                if (resultCode == ResultCode.EntryAlreadyExists || resultCode == ResultCode.UnwillingToPerform)
                {
                    return;
                }

                throw;
            }
        }

        public void Update(string id, IEnumerable<DirectoryAttributeModification> modifications)
        {
            _requestService.MakeModifyRequest(id, modifications);
        }

        public string UpdateDistinguishedName(string oldDistinguishedName, string parentDistinguishedName, string name)
        {
            var prefix = _distinguishedNameBuilderService.BuildNamePrefixByName(name);
            _requestService.MakeModifyDistinguishedName(oldDistinguishedName, parentDistinguishedName, prefix);

            return _distinguishedNameBuilderService.BuildDistinguishedName(name, parentDistinguishedName);
        }

        public void Delete(string distinguishedName)
        {
            _requestService.MakeDeleteRequest(distinguishedName);
        }
    }
}
