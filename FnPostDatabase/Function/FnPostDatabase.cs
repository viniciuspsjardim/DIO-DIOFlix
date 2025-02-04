using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using FnPostDatabase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VinnLib.Services;

namespace FnPostDatabase.Function
{
    public class FnPostDatabase(ILogger<FnPostDatabase> logger, CosmosService cosmosService)
    {
        private readonly ILogger<FnPostDatabase> _logger = logger;
        private readonly CosmosService _cosmosService = cosmosService;

        [Function("movie")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (!req.HasJsonContentType())
            {
                return new BadRequestObjectResult("Content-type deve ser json");
            }

            MovieRequest? movie;

            try
            {
                movie = await JsonSerializer.DeserializeAsync<MovieRequest>(req.Body);
            }
            catch (JsonException ex)
            {
                return new BadRequestObjectResult("Json inválido: " + ex.Message);
            }

            if (movie == null)
            {
                return new BadRequestResult();
            }

            await _cosmosService.Create(movie);

            return new NoContentResult();
        }
    }
}
