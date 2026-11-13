namespace BankingFraud.Api.Models;
public sealed record CreateTransactionRequest(int AccountId, decimal Amount, string Currency, string Merchant, string Location, DateTime OccurredAtUtc, bool UnusualLocation = false);
public sealed record ReviewDecisionRequest(string Decision, string? Note);
public sealed record FraudAssessment(int RiskScore, IReadOnlyList<string> Reasons)
{
    public bool RequiresReview => RiskScore >= 50;
}
