using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
                throw new InvalidOperationException("Checking health check failed");
            }
            
            _logger.LogInformation("Connection established");
            return HealthCheckResult.Healthy("Successfully connected to the database.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database connection check failed.", ex);
        }
    }
}