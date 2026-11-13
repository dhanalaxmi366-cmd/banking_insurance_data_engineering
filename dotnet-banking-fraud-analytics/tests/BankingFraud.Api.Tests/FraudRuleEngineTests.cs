using BankingFraud.Api.Models;
using BankingFraud.Api.Services;
namespace BankingFraud.Api.Tests;
public sealed class FraudRuleEngineTests
{
    private readonly FraudRuleEngine _engine = new();
    [Fact]
    public void HighValueTransaction_IsFlaggedForReview()
    {
        var result = _engine.Assess(new BankTransaction { Amount=1500m }, 0);
        Assert.True(result.RequiresReview);
        Assert.Contains(result.Reasons, reason => reason.Contains("High-value"));
    }
    [Fact]
    public void LowRiskTransaction_DoesNotRequireReview()
    {
        var result = _engine.Assess(new BankTransaction { Amount=25m }, 0);
        Assert.False(result.RequiresReview);
        Assert.Empty(result.Reasons);
    }
    [Fact]
    public void MultipleSignals_AreCombinedAndCappedAtOneHundred()
    {
        var result = _engine.Assess(new BankTransaction { Amount=2000m, UnusualLocation=true }, 5);
        Assert.Equal(100, result.RiskScore);
        Assert.Equal(3, result.Reasons.Count);
    }
}
