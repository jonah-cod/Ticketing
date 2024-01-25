using Serilog;
using Ticketing.Logging;

var builder = WebApplication.CreateBuilder(args);

// // new logger configuration using serilog
// Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/ticketLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

// builder.Host.UseSerilog();



// Add services to the container.

builder.Services.AddControllers(option=>{
    // option.ReturnHttpNotAcceptable=true;
}).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// injecting custom logger
builder.Services.AddSingleton<ILogging, Logging>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
