using System.Threading.Tasks;
using FnGetDatabase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using VinnLib.Services;

namespace FnGetDatabase.Function
{
    public class FnGetDatabase(ILogger<FnGetDatabase> logger, CosmosService cosmosService)
    {
        private readonly ILogger<FnGetDatabase> _logger = logger;
        private readonly CosmosService _cosmosService = cosmosService;

        [Function("FnGetDatabase")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var id = req.Query.Get("id");

            var result = await _cosmosService.Get<Movie>(id!);

            return new OkObjectResult(result);
        }
    }
}
