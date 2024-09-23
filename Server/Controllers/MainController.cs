namespace Server.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class MainController(ILogger<MainController> logger) : ControllerBase
{
    private readonly string path = "../result.json";
    private readonly ILogger<MainController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult> GetResult()
    {
        var bytes = await System.IO.File.ReadAllBytesAsync(path);

        this._logger.LogInformation(
            "[http][{status}] {path} [{date}] => Readed result file",
            [HttpContext.Response.StatusCode, HttpContext.Request.Path.Value, DateTime.UtcNow]);
        return this.File(bytes, MediaTypeNames.Application.Json);
    }

    [HttpGet("download")]
    public async Task<ActionResult> Download()
    {
        var bytes = await System.IO.File.ReadAllBytesAsync(path);

        this._logger.LogInformation(
            "[http][{status}] {path} [{date}] => Readed result file",
            [HttpContext.Response.StatusCode, HttpContext.Request.Path.Value, DateTime.UtcNow]);
        return this.File(bytes, MediaTypeNames.Application.Json, Path.GetFileName(path));
    }
}
