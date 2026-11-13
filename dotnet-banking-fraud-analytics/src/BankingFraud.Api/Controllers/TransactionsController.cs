using BankingFraud.Api.Data;
using BankingFraud.Api.Models;
using BankingFraud.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BankingFraud.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public sealed class TransactionsController(BankingDbContext db, FraudRuleEngine rules) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BankTransaction>>> Get([FromQuery] int? accountId, [FromQuery] ReviewStatus? status, [FromQuery] DateTime? fromUtc, [FromQuery] DateTime? toUtc, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        page = Math.Max(page, 1); pageSize = Math.Clamp(pageSize, 1, 100);
        var query = db.Transactions.AsNoTracking().AsQueryable();
        if (accountId.HasValue) query = query.Where(x => x.AccountId == accountId.Value);
        if (status.HasValue) query = query.Where(x => x.ReviewStatus == status.Value);
        if (fromUtc.HasValue) query = query.Where(x => x.OccurredAtUtc >= fromUtc.Value);
        if (toUtc.HasValue) query = query.Where(x => x.OccurredAtUtc <= toUtc.Value);
        return Ok(await query.OrderByDescending(x => x.OccurredAtUtc).Skip((page-1)*pageSize).Take(pageSize).ToListAsync());
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BankTransaction>> GetById(int id)
    {
        var item = await db.Transactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return item is null ? NotFound() : Ok(item);
    }
    [HttpPost]
    public async Task<ActionResult<BankTransaction>> Create(CreateTransactionRequest request)
    {
        if (request.AccountId <= 0 || request.Amount <= 0 || request.Amount > 1_000_000m) return BadRequest("AccountId and Amount must be positive; Amount cannot exceed 1,000,000.");
        if (string.IsNullOrWhiteSpace(request.Currency) || request.Currency.Trim().Length != 3) return BadRequest("Currency must be a three-letter code.");
        if (string.IsNullOrWhiteSpace(request.Merchant) || request.Merchant.Length > 120 || string.IsNullOrWhiteSpace(request.Location) || request.Location.Length > 120) return BadRequest("Merchant and Location are required (maximum 120 characters).");
        if (request.OccurredAtUtc > DateTime.UtcNow.AddMinutes(5)) return BadRequest("OccurredAtUtc cannot be in the future.");
        var recentCount = await db.Transactions.CountAsync(x => x.AccountId == request.AccountId && x.OccurredAtUtc >= request.OccurredAtUtc.AddHours(-1) && x.OccurredAtUtc <= request.OccurredAtUtc);
        var item = new BankTransaction { AccountId=request.AccountId, Amount=request.Amount, Currency=request.Currency.Trim().ToUpperInvariant(), Merchant=request.Merchant.Trim(), Location=request.Location.Trim(), OccurredAtUtc=DateTime.SpecifyKind(request.OccurredAtUtc, DateTimeKind.Utc), UnusualLocation=request.UnusualLocation };
        var assessment = rules.Assess(item, recentCount);
        item.RiskScore = assessment.RiskScore; item.RiskReasons = string.Join("; ", assessment.Reasons);
        item.ReviewStatus = assessment.RequiresReview ? ReviewStatus.Pending : ReviewStatus.NotRequired;
        db.Transactions.Add(item); await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id=item.Id }, item);
    }
}
