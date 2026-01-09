#pragma warning disable CA1506 // Avoid excessive class coupling
using Serilog;
using CustomerValidator.Application;
using CustomerValidator.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Logger
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Controllers
builder.Services.AddControllers(c => c.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

/// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application + Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middlewares básicos
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
public partial class Program { }

#pragma warning restore CA1506 // Avoid excessive class coupling
