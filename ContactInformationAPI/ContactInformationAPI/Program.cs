using ContactInformationAPI.Middlewares;
using ContactInformationAPI.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();



string logPath = builder.Configuration.GetSection("Logging:LogPath").Value;
var _logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(logPath)
    .CreateLogger();

builder.Logging.AddSerilog(_logger);

builder.Services.AddTransient<IContactDB, ContactRespository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("corsploicy", bulid =>
    {
        bulid.WithOrigins("http://localhost:4200", "https://ui.showexy.com").
        AllowAnyMethod().AllowAnyHeader().WithMethods("GET", "POST", "DELETE", "PUT");
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseMiddleware<ExcepationMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseCors("corsploicy");
app.Run();
