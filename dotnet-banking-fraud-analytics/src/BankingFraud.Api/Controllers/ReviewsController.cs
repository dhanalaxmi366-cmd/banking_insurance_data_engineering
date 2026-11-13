using BankingFraud.Api.Data;
using BankingFraud.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BankingFraud.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public sealed class ReviewsController(BankingDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BankTransaction>>> GetQueue()
    {
        var queue = await db.Transactions.AsNoTracking().Where(x => x.ReviewStatus == ReviewStatus.Pending).OrderByDescending(x => x.RiskScore).ThenBy(x => x.OccurredAtUtc).Take(100).ToListAsync();
        return Ok(queue);
    }
    [HttpPost("{transactionId:int}")]
    public async Task<IActionResult> Decide(int transactionId, ReviewDecisionRequest request)
    {
        var decision = request.Decision?.Trim().ToLowerInvariant();
        if (decision is not ("approved" or "declined")) return BadRequest("Decision must be 'approved' or 'declined'.");
        if (request.Note?.Length > 1000) return BadRequest("Note cannot exceed 1000 characters.");
        var item = await db.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);
        if (item is null) return NotFound();
        if (item.ReviewStatus != ReviewStatus.Pending) return Conflict("Only pending transactions can be reviewed.");
        item.ReviewStatus = decision == "approved" ? ReviewStatus.Approved : ReviewStatus.Declined;
        item.AnalystNote = request.Note?.Trim(); item.ReviewedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(); return Ok(item);
    }
}
