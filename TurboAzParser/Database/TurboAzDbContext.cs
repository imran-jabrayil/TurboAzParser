using Microsoft.EntityFrameworkCore;
using TurboAzParser.Models;
using TurboAzParser.Models.Enums;

namespace TurboAzParser.Database;

public sealed class TurboAzDbContext(DbContextOptions<TurboAzDbContext> options) : DbContext(options)
{
    public DbSet<UrlHistory> UrlHistory { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrlHistory>(urlHistory =>
        {
            urlHistory.HasKey(x => x.Id);
            
            urlHistory.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            urlHistory.Property(x => x.Url)
                .HasColumnName("url")
                .HasMaxLength(50)
                .IsRequired();
            
            urlHistory.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<byte>()
                .IsRequired()
                .HasDefaultValue(ParsingStatus.Failed);
            
            urlHistory.Property(e => e.CreatedAt)
                .HasColumnName("createdAt")
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSDATETIME()");

            urlHistory.Property(e => e.UpdatedAt)
                .HasColumnName("updatedAt")
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSDATETIME()");

            urlHistory.HasIndex(e => e.Url)
                .HasDatabaseName("I_UrlHistory_Url")
                .IsUnique()
                .IncludeProperties(e => new { e.Status, e.CreatedAt, e.UpdatedAt });
        });
        
        base.OnModelCreating(modelBuilder);
    }
}