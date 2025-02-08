using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TurboAzParser.Models;

namespace TurboAzParser.HealthChecks;

public sealed class DbContextHealthCheck<TContext>(
    TContext dbContext, 
    ILogger<DbContextHealthCheck<TContext>> logger) : IHealthCheck
    where TContext : DbContext
{
    private readonly TContext _dbContext = dbContext;
    private readonly ILogger<DbContextHealthCheck<TContext>> _logger = logger;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking health check");
            
            bool canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);

            if (!canConnect)
            {
                throw new InvalidOperationException("Could not connect to database");
            }
            
            _logger.LogInformation("Connection established");
            
            bool _ = await _dbContext.Set<UrlHistory>().AnyAsync(cancellationToken);
            
            return HealthCheckResult.Healthy("Database is healthy");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database check failed.", ex);
        }
    }
}