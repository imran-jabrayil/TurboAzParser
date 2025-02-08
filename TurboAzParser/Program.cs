using Microsoft.EntityFrameworkCore;
using TurboAzParser.Clients;
using TurboAzParser.Clients.Abstractions;
using TurboAzParser.Clients.Settings;
using TurboAzParser.Database;
using TurboAzParser.HealthChecks;
using TurboAzParser.Mappers;
using TurboAzParser.Services;
using TurboAzParser.Services.Abstractions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddHttpClient<ITurboAzClient, TurboAzClient>();
builder.Services.Configure<TurboAzClientSettings>(builder.Configuration.GetSection("TurboAzClient"));

builder.Services.AddDbContext<TurboAzDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TurboAzConnectionString")));

builder.Services.AddHealthChecks()
    .AddCheck<DbContextHealthCheck<TurboAzDbContext>>("database");

builder.Services.AddScoped<ITurboAzService, TurboAzService>();

builder.Services.AddAutoMapper(typeof(CarInfoProfile));

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();