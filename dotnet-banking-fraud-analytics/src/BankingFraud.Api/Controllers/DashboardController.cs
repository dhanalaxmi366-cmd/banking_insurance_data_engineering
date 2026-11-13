using BankingFraud.Api.Data;
using BankingFraud.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BankingFraud.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public sealed class DashboardController(BankingDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = db.Transactions.AsNoTracking();
        var total = await query.CountAsync();
        var amount = await query.SumAsync(x => (decimal?)x.Amount) ?? 0m;
        var pending = await query.CountAsync(x => x.ReviewStatus == ReviewStatus.Pending);
        var flagged = await query.CountAsync(x => x.RiskScore >= 50);
        var byStatus = await query.GroupBy(x => x.ReviewStatus).Select(g => new { status=g.Key.ToString(), count=g.Count() }).ToListAsync();
        return Ok(new { totalTransactions=total, totalAmount=amount, pendingReviews=pending, flaggedTransactions=flagged, transactionsByStatus=byStatus });
    }
}
