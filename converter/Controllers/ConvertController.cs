using converter.Converter.Core;
using converter.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace converter.Controllers
{
    public class ConvertController : Controller
    {
        private readonly IConvertRepository _convertRepo;
        private readonly ConverterFileManagerBase<Models.Convert> _manager;

        public ConvertController(IConvertRepository convertRepository, ConverterFileManagerBase<Models.Convert> manager)
        {
            _convertRepo = convertRepository;
            _manager = manager;
        }

        [HttpGet]
        [Route("[controller]/download/{id}/{format}")]
        public async Task<IActionResult> DownloadJsonAsync(int id, string format)
        {
            Models.Convert? temp = await _convertRepo.GetAsync(id);

            if (temp is null)
            {
                return NotFound();
            }

            string filePath = $"~/Files/{temp.Id}.{format}";

            string fileType = $"application/{format}";

            string fileName = $"{temp.FileName}.{format}";

            return File(filePath, fileType, fileName);
        }

        [HttpPost]
        [Route("[controller]/upload")]
        public async Task<IActionResult> AddFileAsync(IFormFile uploadedFile)
        {
            if (uploadedFile == null)
            {
                return BadRequest();
            }

            string path = Path.Combine("~/Files/", uploadedFile.FileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            Models.Convert conv = new()
            {
                FileName = uploadedFile.FileName,
                ContentType = uploadedFile.ContentType,
            };

            var convEnriry = await _convertRepo.AddAsync(conv);
            await _manager.AddAsync(convEnriry);
            return Ok();
        }
    }
}
