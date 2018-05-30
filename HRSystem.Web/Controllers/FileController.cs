using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Web.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IMediator _mediator;

        public FileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(string employee, int attributeInfoId, IFormFile file)
        {
            var attributeDirectory = $"EDocs/{attributeInfoId}";
            if (!Directory.Exists(attributeDirectory))
            {
                Directory.CreateDirectory(attributeDirectory);
            }

            var path = $"{attributeDirectory}/{employee}{Path.GetExtension(file.FileName)}";
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream).ConfigureAwait(false);
            }

            return Ok();
        }

        [HttpGet("download/{employee}/{attributeInfoId}")]
        public async Task<IActionResult> Download(string employee, int attributeInfoId)
        {
            var attributeDirectory = $"EDocs\\{attributeInfoId}";
            if (!Directory.Exists(attributeDirectory))
            {
                return Content("ФАЙЛА НЕТ.");
            }

            var fileNameWithoutExtension = $"{attributeDirectory}\\{employee}";
            var filePathes = Directory.GetFiles(attributeDirectory);
            var path = filePathes.FirstOrDefault(file => file.StartsWith(fileNameWithoutExtension));
            if (string.IsNullOrEmpty(path))
            {
                return Content("ФАЙЛА НЕТ.");
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;

            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        [HttpGet("exists")]
        public bool Exists(string employee, int attributeInfoId)
        {
            var attributeDirectory = $"EDocs\\{attributeInfoId}";
            if (!Directory.Exists(attributeDirectory))
            {
                return false;
            }

            var fileNameWithoutExtension = $"{attributeDirectory}\\{employee}";
            var filePathes = Directory.GetFiles(attributeDirectory);
            var path = filePathes.FirstOrDefault(file => file.StartsWith(fileNameWithoutExtension));
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            return true;
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.TryGetValue(ext, out var result) ? result : "application/octet-stream";
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}