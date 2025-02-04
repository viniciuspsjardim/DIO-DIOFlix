using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VinnLib.Services;

namespace FnPostDataStorage.Function
{
    public class FnPostDataStorage(ILogger<FnPostDataStorage> logger, BlobService blobService)
    {
        private readonly ILogger<FnPostDataStorage> _logger = logger;
        private readonly BlobService _blobService = blobService;

        [Function("dataStorage")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (!req.Form.TryGetValue("fileType", out var fileTypeValues))
            {
                return new BadRequestObjectResult("O parametro fileType deve ser informado");
            }

            string fileType = fileTypeValues.FirstOrDefault() ?? string.Empty;

            if (fileType != "imagem" && fileType != "video")
            {
                return new BadRequestObjectResult("O paramtro fileType deve ser \"imagem\" ou \"video\"");
            }

            var file = req.Form.Files["file"];

            if (file == null || file.Length == 0)
            {
                return new BadRequestObjectResult("O parametro file deve ser informado e não pode ser nulo");
            }

            using (var stream = file.OpenReadStream())
            {
                switch (fileType)
                {
                    case "imagem":
                        await _blobService.AddImage(file.FileName, stream);
                        break;

                    case "video":
                        await _blobService.AddVideo(file.FileName, stream);
                        break;
                }
            }

            return new NoContentResult();
        }
    }
}
