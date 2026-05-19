using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MilhasAPI.Data;
using MilhasAPI.Jobs;
using MilhasAPI.Repositories;
using MilhasAPI.Repositories.Interfaces;
using MilhasAPI.Scrapers;
using MilhasAPI.Scrapers.Interfaces;
using MilhasAPI.Services;
using MilhasAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// ── Swagger ────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── CORS — permite o frontend React acessar a API ──────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",   // Vite dev server (padrão)
                "http://localhost:4173",   // Vite preview
                "http://localhost:3000"    // fallback
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ── Banco de dados ─────────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ───────────────────────────────────────────────────
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<ICreditCardRepository, CreditCardRepository>();
builder.Services.AddScoped<IRewardTransactionRepository, RewardTransactionRepository>();

// ── Services ───────────────────────────────────────────────────────
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<ICreditCardService, CreditCardService>();
builder.Services.AddScoped<IRewardTransactionService, RewardTransactionService>();

// ── HttpClient para scrapers ───────────────────────────────────────
var scraperConfig = builder.Configuration.GetSection("Scraper");
builder.Services.AddHttpClient("ScraperClient", client =>
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        scraperConfig["UserAgent"] ?? "Mozilla/5.0");
    client.Timeout = TimeSpan.FromSeconds(
        scraperConfig.GetValue<int>("TimeoutSeconds", 30));
});

// ── Scrapers ───────────────────────────────────────────────────────
// Hoje uma única fonte SSR (MelhoresDestinos) cobre múltiplos programas.
// Novos scrapers podem ser adicionados aqui — o MilesMonitorService deduplica
// por programa mantendo a menor cotação.
builder.Services.AddScoped<IMilesScraper, MelhoresDestinosScraper>();

// ── Monitor ────────────────────────────────────────────────────────
builder.Services.AddScoped<IMilesMonitorService, MilesMonitorService>();

// ── Background job ────────────────────────────────────────────────
builder.Services.AddHostedService<MilesScraperJob>();

var app = builder.Build();

// ── Seed do banco ──────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// ── Swagger UI ─────────────────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MilhasAPI v1"));

if (app.Environment.IsDevelopment())
    app.MapSwagger();

// ── CORS antes de tudo ─────────────────────────────────────────────
app.UseCors("FrontendPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
