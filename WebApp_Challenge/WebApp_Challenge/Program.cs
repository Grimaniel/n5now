using Microsoft.EntityFrameworkCore;
using WebApp_Challenge.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Nest; // Para Elasticsearch
using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .WriteTo.Console()
    .MinimumLevel.Information());

// Configurar DbContext sql server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Elasticsearch
var elasticsearchSettings = new ConnectionSettings(new Uri("http://localhost:9200"))
    .DefaultIndex("permissions");
var elasticClient = new ElasticClient(elasticsearchSettings);
builder.Services.AddSingleton<IElasticClient>(elasticClient);

// Configurar Kafka
builder.Services.AddSingleton<IProducer<Null, string>>(provider =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    };
    return new ProducerBuilder<Null, string>(config).Build();
});

//Configuracion de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add services to the container.


// Agregar controladores
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => Results.Ok("API is running correctly"));
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting(); //se agrego en caso de errores
app.UseAuthorization();

app.MapControllers();

app.Run();

