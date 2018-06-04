using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HRSystem.Commands.AddDocument;
using HRSystem.Commands.DeleteAttribute;
using HRSystem.Commands.DeleteDocument;
using HRSystem.Common.Validation;
using HRSystem.Queries.GetDocument;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRSystem.Web.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly IMediator _mediator;

        public DocumentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(string employee, int attributeInfoId, IFormFile file)
        {
            byte[] byteArray;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream).ConfigureAwait(false);
                byteArray = stream.ToArray();
            }

            try
            {
                await _mediator.Send(new AddDocumentCommand
                {
                    EmployeeLogin = employee,
                    AttributeInfoId = attributeInfoId,
                    Document = new Domain.Document(file.FileName, byteArray)
                });
            }
            catch (ValidationException e)
            {
                return BadRequest(e.ToResponse());
            }

            return Ok();
        }

        [HttpGet("download/{employeeLogin}/{attributeInfoId}")]
        public async Task<IActionResult> Download(string employeeLogin, int attributeInfoId)
        {
            try
            {
                var response =
                    await _mediator.Send(new GetDocumentQuery
                        {
                            EmployeeLogin = employeeLogin,
                            AttributeInfoId = attributeInfoId
                        })
                        .ConfigureAwait(false);
                var memory = new MemoryStream(response.Document.Content) {Position = 0};

                return File(memory, GetContentType(response.Document.Name), Path.GetFileName(response.Document.Name));
            }
            catch (ValidationException)
            {
                return Content("Такой файл не найден.");
            }
        }

        [HttpDelete("delete/{employeeLogin}/{attributeInfoId}")]
        public async Task<IActionResult> Delete(string employeeLogin, int attributeInfoId)
        {
            try
            {
                await _mediator
                    .Send(new DeleteDocumentCommand {EmployeeLogin = employeeLogin, AttributeInfoId = attributeInfoId})
                    .ConfigureAwait(false);
                return Ok();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.ToResponse());
            }
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