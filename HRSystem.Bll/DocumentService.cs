using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using HRSystem.Core;
using HRSystem.Domain;
using HRSystem.Domain.Attributes.Base;

namespace HRSystem.Bll
{
    public class DocumentService : IDocumentService
    {
        private static readonly object LockObject = new object();
        
        public void Save(Employee employee, AttributeInfo attributeInfo, Document document)
        {
            var attributeDirectory = $"EDocs\\{attributeInfo.Id}";
            if (!Directory.Exists(attributeDirectory))
            {
                Directory.CreateDirectory(attributeDirectory);
            }

            var path = $"{attributeDirectory}/{employee.Login}{Path.GetExtension(document.Name)}";
            lock (LockObject)
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    fileStream.Write(document.Content, 0, document.Content.Length);
                }
            }
        }

        public Document Load(Employee employee, AttributeInfo attributeInfo)
        {
            var path = GetPath(employee, attributeInfo);

            byte[] content;
            lock (LockObject)
            {
                content = File.ReadAllBytes(path);
            }
            
            return new Document(Path.GetFileName(path), content);
        }

        private static string GetPath(Employee employee, AttributeInfo attributeInfo)
        {
            var attributeDirectory = $"EDocs\\{attributeInfo.Id}";
            if (!Directory.Exists(attributeDirectory))
            {
                throw new InvalidOperationException($"File '{employee.Login}\\{attributeInfo.Name}' not found.");
            }

            var fileNameWithoutExtension = $"{attributeDirectory}\\{employee.Login}";
            var filePathes = Directory.GetFiles(attributeDirectory);
            var path = filePathes.FirstOrDefault(file => file.StartsWith(fileNameWithoutExtension));
            return path;
        }

        public bool IsExists(Employee employee, AttributeInfo attributeInfo)
        {
            var attributeDirectory = $"EDocs\\{attributeInfo.Id}";
            if (!Directory.Exists(attributeDirectory))
            {
                return false;
            }

            var fileNameWithoutExtension = $"{attributeDirectory}\\{employee.Login}";
            var filePathes = Directory.GetFiles(attributeDirectory);
            var path = filePathes.FirstOrDefault(file => file.StartsWith(fileNameWithoutExtension));
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            return true;
        }

        public void Delete(Employee employee, AttributeInfo attributeInfo)
        {
            var path = GetPath(employee, attributeInfo);
            if (!File.Exists(path))
            {
                throw new InvalidOperationException("File not found.");
            }

            lock (LockObject)
            {
                File.Delete(path);
            }
        }
    }
}