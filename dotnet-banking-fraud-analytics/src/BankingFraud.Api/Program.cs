using BankingFraud.Api.Data;
using BankingFraud.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<FraudRuleEngine>();

var connectionString = builder.Configuration.GetConnectionString("BankingDb")
    ?? "Server=(localdb)\\mssqllocaldb;Database=BankingFraudDemo;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
builder.Services.AddDbContext<BankingDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddCors(options => options.AddPolicy("LocalFrontend", policy =>
    policy.WithOrigins("http://localhost:5173", "https://localhost:5173").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection();
app.UseCors("LocalFrontend");
app.MapControllers();

// Demo bootstrap only. Use reviewed EF Core migrations in a deployed system.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BankingDbContext>();
    await db.Database.EnsureCreatedAsync();
    await DemoData.SeedAsync(db);
}
app.Run();
public partial class Program { }
