using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace OneInc.ADEditor.Dal.Extensions
{
    public static class DirectoryAttributeModificationExtensions
    {
        public static void AddModificationIfDataChanged<T>(
            this ICollection<DirectoryAttributeModification> modifications,
            string attributeName,
            T updatedEntity,
            T oldEntity,
            Func<T, string> getValueFunc = null)
            where T : class
        {
            var updatedValue = GetValueAsNullIfEmpty(updatedEntity, getValueFunc);
            var oldValue = GetValueAsNullIfEmpty(oldEntity, getValueFunc);
            if (updatedValue == oldValue)
            {
                return;
            }

            var hasOldValue = !string.IsNullOrEmpty(oldValue);
            var hasUpdatedValue = !string.IsNullOrEmpty(updatedValue);

            var modification = new DirectoryAttributeModification { Name = attributeName };
            if (hasUpdatedValue)
            {
                modification.Operation = DirectoryAttributeOperation.Replace;
                modification.Add(updatedValue);
            }
            else if (hasOldValue)
            {
                modification.Operation = DirectoryAttributeOperation.Delete;
            }

            modifications.Add(modification);
        }

        private static string GetValueAsNullIfEmpty<T>(T obj, Func<T, string> getValueFunc)
            where T : class
        {
            if (obj == null)
            {
                return null;
            }

            var value = getValueFunc(obj);

            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}
