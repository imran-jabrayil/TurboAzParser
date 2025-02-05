using TurboAzParser.Client;
using TurboAzParser.Client.Abstractions;
using TurboAzParser.Client.Settings;
using TurboAzParser.Services;
using TurboAzParser.Services.Abstractions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddHttpClient<ITurboAzClient, TurboAzClient>();
builder.Services.Configure<TurboAzClientSettings>(builder.Configuration.GetSection("TurboAzClient"));

builder.Services.AddScoped<ITurboAzService, TurboAzService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();