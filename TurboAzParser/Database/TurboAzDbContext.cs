using Microsoft.EntityFrameworkCore;

namespace TurboAzParser.Database;

public sealed class TurboAzDbContext(DbContextOptions<TurboAzDbContext> options) : DbContext(options)
{
    
}