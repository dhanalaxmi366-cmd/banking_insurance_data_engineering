using BankingFraud.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace BankingFraud.Api.Data;
public sealed class BankingDbContext(DbContextOptions<BankingDbContext> options) : DbContext(options)
{
    public DbSet<BankTransaction> Transactions => Set<BankTransaction>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<BankTransaction>();
        entity.ToTable("BankTransactions");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.Amount).HasPrecision(18, 2);
        entity.Property(x => x.Currency).HasMaxLength(3).IsRequired();
        entity.Property(x => x.Merchant).HasMaxLength(120).IsRequired();
        entity.Property(x => x.Location).HasMaxLength(120).IsRequired();
        entity.Property(x => x.RiskReasons).HasMaxLength(1000);
        entity.Property(x => x.AnalystNote).HasMaxLength(1000);
        entity.HasIndex(x => new { x.AccountId, x.OccurredAtUtc });
        entity.HasIndex(x => x.ReviewStatus);
    }
}
public static class DemoData
{
    public static async Task SeedAsync(BankingDbContext db)
    {
        if (await db.Transactions.AnyAsync()) return;
        var now = DateTime.UtcNow;
        db.Transactions.AddRange(
            new BankTransaction { AccountId=1001, Amount=42.50m, Merchant="Neighborhood Market", Location="Phoenix, AZ", Currency="USD", OccurredAtUtc=now.AddHours(-5) },
            new BankTransaction { AccountId=1001, Amount=1250m, Merchant="Demo Electronics", Location="Phoenix, AZ", Currency="USD", OccurredAtUtc=now.AddHours(-2) },
            new BankTransaction { AccountId=1002, Amount=780m, Merchant="Online Travel Demo", Location="Unknown Region", Currency="USD", OccurredAtUtc=now.AddMinutes(-20), UnusualLocation=true }
        );
        await db.SaveChangesAsync();
    }
}
