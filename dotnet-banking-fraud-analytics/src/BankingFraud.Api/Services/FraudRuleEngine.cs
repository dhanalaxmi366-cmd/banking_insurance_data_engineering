using BankingFraud.Api.Models;
namespace BankingFraud.Api.Services;
public sealed class FraudRuleEngine
{
    public FraudAssessment Assess(BankTransaction transaction, int recentTransactionsInLastHour)
    {
        var reasons = new List<string>();
        var score = 0;
        if (transaction.Amount >= 1000m) { score += 40; reasons.Add("High-value transaction (>= $1,000)"); }
        if (recentTransactionsInLastHour >= 4) { score += 35; reasons.Add("High transaction frequency (4+ in one hour)"); }
        if (transaction.UnusualLocation) { score += 35; reasons.Add("Location flagged as unusual for this demo"); }
        return new FraudAssessment(Math.Min(score, 100), reasons);
    }
}
