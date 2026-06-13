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
// Origens vêm da configuração (AllowedOrigins, separadas por vírgula).
// Em dev caímos no fallback de localhost; em produção definir a var de ambiente
// AllowedOrigins com a URL pública do frontend.
var configuredOrigins = (builder.Configuration["AllowedOrigins"] ?? "")
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

var allowedOrigins = configuredOrigins.Length > 0
    ? configuredOrigins
    : new[]
    {
        "http://localhost:5173",   // Vite dev server (padrão)
        "http://localhost:4173",   // Vite preview
        "http://localhost:3000"    // fallback
    };

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
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
builder.Services.AddScoped<IMilesGoalRepository, MilesGoalRepository>();

// ── Options ────────────────────────────────────────────────────────
builder.Services.Configure<MilhasAPI.Configuration.EmailOptions>(
    builder.Configuration.GetSection(MilhasAPI.Configuration.EmailOptions.SectionName));

// ── Services ───────────────────────────────────────────────────────
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<ICreditCardService, CreditCardService>();
builder.Services.AddScoped<IRewardTransactionService, RewardTransactionService>();
builder.Services.AddScoped<IMilesGoalService, MilesGoalService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<IEmailTemplateRenderer, EmailTemplateRenderer>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IPasswordResetService, PasswordResetService>();

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

// ── Estimador (fallback) e cache de cotações ───────────────────────
builder.Services.AddSingleton<IMilesQuoteEstimator, MilesQuoteEstimator>();
builder.Services.AddSingleton<IQuotesCache, QuotesCache>();

// ── Monitor ────────────────────────────────────────────────────────
builder.Services.AddScoped<IMilesMonitorService, MilesMonitorService>();

// ── Background job ────────────────────────────────────────────────
builder.Services.AddHostedService<MilesScraperJob>();

var app = builder.Build();

// ── Migrations do banco ────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// ── Swagger UI ─────────────────────────────────────────────────────
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MilhasAPI v1"));

if (app.Environment.IsDevelopment())
    app.MapSwagger();

// ── CORS antes de tudo ─────────────────────────────────────────────
app.UseCors("FrontendPolicy");

// Em produção (Railway/etc.) o TLS é terminado na borda e o container recebe
// HTTP; o redirect ativo causa loop. Mantemos só em dev.
if (app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
