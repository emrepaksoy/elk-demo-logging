using DemoLogging.Log;
using DemoLogging.Log.LogModels;
using System.Text;

namespace DemoLogging.Extensions
{
    public class RequestResponseExtension
    {
        readonly RequestDelegate _next;
        readonly IElasticSearchService<RequestLogModel> _elasticSearchService;
        readonly IConfiguration _configuration;
        public RequestResponseExtension(RequestDelegate next, IConfiguration configuration, IElasticSearchService<RequestLogModel> elasticSearchService)
        {

            _next = next;
            _configuration = configuration;
            _elasticSearchService = elasticSearchService;
        }

        public async Task Invoke(HttpContext httpContext)
        {
    

            var originalBodyStream = httpContext.Response.Body;

            var tempStream = new MemoryStream();
            httpContext.Response.Body = tempStream;

            RequestLogModel requestLogModel = new RequestLogModel();

            _elasticSearchService.CheckExistsAndInsertLog(requestLogModel, _configuration["ElasticSearchConnectionSettings:RequestIndex"]);

            await _next.Invoke(httpContext);

            // response
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
            String reader = await new StreamReader(httpContext.Response.Body, Encoding.UTF8).ReadToEndAsync();
            httpContext.Response.Body.Seek(0, SeekOrigin.Begin);


            

            await httpContext.Response.Body.CopyToAsync(originalBodyStream);
        }

    }
}
