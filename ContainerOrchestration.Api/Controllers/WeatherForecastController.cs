using Microsoft.AspNetCore.Mvc;

namespace ContainerOrchestration.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly InstanceService _instanceService;
    private readonly HttpClient _httpClient;

    public WeatherForecastController(
        InstanceService instanceService,
        IHttpClientFactory factory)
    {
        _instanceService = instanceService;
        _httpClient = factory.CreateClient(InternalServiceSettings.Key);
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    [HttpGet("MachineInfo")]
    public IActionResult GetMachineInfo()
    {
        return Ok(new
        {
            Environment.MachineName,
            Environment.ProcessId,
            Environment.CurrentManagedThreadId,
            Environment.OSVersion,
            Environment.Is64BitOperatingSystem,
            Environment.Version,
            _instanceService.InstanceId,
        });
    }
    
    [HttpGet("CallInternalService")]
    public async Task<IActionResult> CallInternalService()
    {
        var httpResponseMessage = 
            await _httpClient.GetAsync("WeatherForecast/MachineInfo");

        var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

        return Ok(responseString);
    }
}