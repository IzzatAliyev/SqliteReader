namespace Server.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class MainController(ILogger<MainController> logger) : ControllerBase
{
    private readonly ILogger<MainController> _logger = logger;

    [HttpGet("result")]
    public async Task<ActionResult> GetResult()
    {
        var bytes = await System.IO.File.ReadAllBytesAsync("../result.json");

        this._logger.LogInformation($"[http][{HttpContext.Response.StatusCode}] {HttpContext.Request.Path.Value} [{DateTime.UtcNow}] => Readed result file");
        return this.File(bytes, MediaTypeNames.Application.Json);
    }
}
