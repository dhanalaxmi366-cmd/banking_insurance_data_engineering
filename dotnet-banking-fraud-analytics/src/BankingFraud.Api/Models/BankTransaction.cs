namespace BankingFraud.Api.Models;
public enum ReviewStatus { NotRequired, Pending, Approved, Declined }
public sealed class BankTransaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Merchant { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime OccurredAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public bool UnusualLocation { get; set; }
    public int RiskScore { get; set; }
    public string RiskReasons { get; set; } = string.Empty;
    public ReviewStatus ReviewStatus { get; set; } = ReviewStatus.NotRequired;
    public string? AnalystNote { get; set; }
    public DateTime? ReviewedAtUtc { get; set; }
}
