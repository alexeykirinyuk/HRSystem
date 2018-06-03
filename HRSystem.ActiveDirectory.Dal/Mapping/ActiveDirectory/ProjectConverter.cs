using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.SharePoint.Client;
using OneInc.ADEditor.Models;

using static OneInc.ADEditor.SharePoint.SharePointConstants;
using User = OneInc.ADEditor.Models.User;

namespace OneInc.ADEditor.SharePoint.Mapping.Converters
{
    internal class ProjectConverter : ITypeConverter<ListItem, Project>
    {
        public Project Convert(ListItem source, Project destination, ResolutionContext context)
        {
            return new Project
            {
                Name = source[ProjectEntity.Name] as string,
                ProjectManager = GetUser(source, ProjectEntity.ProjectManager),
                ProjectLeads = GetUsers(source, ProjectEntity.ProjectLeads),
                LeadBusinessAnalysts = GetUsers(source, ProjectEntity.LeadBusinessAnalysts),
                Members = GetUsers(source, ProjectEntity.Members)
            };
        }

        private static ICollection<User> GetUsers(ListItem source, string fieldName)
        {
            var entities = source[fieldName] as IEnumerable<FieldUserValue>;

            return Mapper.Map<ICollection<User>>(entities);
        }

        private static User GetUser(ListItem source, string fieldName)
        {
            var entity = source[fieldName] as FieldUserValue;

            return Mapper.Map<User>(entity);
        }
    }
}