using API_POUPA_FACIL.Configuration;
using API_POUPA_FACIL.Context;
using API_POUPA_FACIL.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
    builder.WebHost.UseUrls($"http://+:{port}");

builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);
builder.Services.AddPoupaFacil(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseForwardedHeaders();
app.UseExceptionHandler();
app.UseRouting();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    headers.TryAdd("X-Content-Type-Options", "nosniff");
    headers.TryAdd("Referrer-Policy", "no-referrer");
    headers.TryAdd("X-Frame-Options", "DENY");
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    // Fail before touching the database when the signing key is missing or revoked.
    _ = scope.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>().Value;

    if (app.Configuration.GetValue("Database:ApplyMigrations", true))
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseMigration");
        var db = scope.ServiceProvider.GetRequiredService<BaseContext>();
        logger.LogInformation("Applying database migrations.");
        db.Database.Migrate();
    }
}

app.Run();

public partial class Program
{
}
