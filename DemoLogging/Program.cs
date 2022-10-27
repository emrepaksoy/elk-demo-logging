using DemoLogging.Extensions;
using DemoLogging.Log;
using DemoLogging.Log.LogModel;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.Elasticsearch;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(typeof(IElasticSearchService<>), typeof(ElasticSearchService<>));
builder.Services.AddSingleton<ElasticSearchClientProvider>();

//Logger log = new LoggerConfiguration()
//    .MinimumLevel.Information()
//    .Enrich.FromLogContext()
//    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticSearchConnectionSettings:url"]))
//    {
//        AutoRegisterTemplate = true,
//        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
//        IndexFormat = builder.Configuration["ElasticSearchConnectionSettings:Response"]

//    }).CreateLogger();

//builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua"); // kullanýcý ya dair bilgileri getirir.
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler(app.Services.GetRequiredService<IElasticSearchService<ErrorLogModel>>(),app.Services.GetRequiredService<IConfiguration>());

app.UseMiddleware<RequestResponseExtension>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
