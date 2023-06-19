using Microsoft.AspNetCore.Mvc;
using PMS_API.Repository;
using PMS_API.SupportModel;

namespace PMS_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IEmailService _emailService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

            var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();
            
            var message = new Message(new string[] { "hearthin2@gmail.com" }, "Test mail with Attachments", "This is the content from our mail with attachments.", files,null);
             _emailService.SendEmail(message);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}