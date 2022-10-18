using DemoLogging.Log;
using DemoLogging.Log.LogModel;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace DemoLogging.Extensions
{
    public static class GlobalExceptionHandlerExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication application, IElasticSearchService<ErrorLogModel> elasticSearchService, IConfiguration configuration)
        {
            application.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        //logger.LogError(contextFeature.Error.Message);

                        //await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        //{
                        //    StatusCode = context.Response.StatusCode,
                        //    Message = contextFeature.Error.Message,
                        //    Title = "Hata alındı!"
                        //})); ;
                       
                        
                        ErrorLogModel log = new ErrorLogModel();
                        log.StatusCode = context.Response.StatusCode;
                        log.Message = contextFeature.Error.Message;
                        elasticSearchService.CheckExistsAndInsertLog(log, configuration["ElasticSearchConnectionSettings:ErrorIndex"]);


                    }
                });
            });
        }
    }
}
