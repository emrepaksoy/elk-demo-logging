using DemoLogging.Log;
using DemoLogging.Log.LogModel;
using Microsoft.AspNetCore.Mvc;

namespace DemoLogging.Controllers
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
        readonly IElasticSearchService<ErrorLogModel> _elasticSearchService;
        readonly IConfiguration _configuration;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IElasticSearchService<ErrorLogModel> elasticSearchService, IConfiguration configuration)
        {

            _logger = logger;
            _elasticSearchService = elasticSearchService;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            //try
            //{
            //    throw new StackOverflowException();
            //}
            //catch (Exception ex)
            //{
            //    ErrorLogModel log = new ErrorLogModel();
            //    log.Action = "Get";
            //    log.Controller = "Weather";
            //    log.Message = "hata alýndý !!!!!";
            //    log.PostDate = DateTime.Now;
            //    var w32ex = ex as System.ComponentModel.Win32Exception;
            //    if (w32ex == null)
            //    {
            //        w32ex = ex.InnerException as System.ComponentModel.Win32Exception;
            //    }
            //    if (w32ex != null)
            //    {
            //        int code = w32ex.ErrorCode;
            //        log.ErrorCode = code;
            //    }
            //    else
            //    {
            //        log.ErrorCode = ex.HResult;
            //    }
            //    _elasticSearchService.CheckExistsAndInsertLog(log,_configuration["ElasticSearchConnectionSettings:ErrorIndex"]);
            //}
            
            //throw new Exception();

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