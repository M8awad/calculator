using CompanyCalculator.Api.Interfaces;
using CompanyCalculator.Api.Services;
using CompanyCalculator.Core.Interfaces;
using CompanyCalculator.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection registrations
builder.Services.AddScoped<ICalculatorService, CalculatorService>();
builder.Services.AddScoped<ILoggingService, LoggingService>();
builder.Services.AddScoped<ICalculator, Calculator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
