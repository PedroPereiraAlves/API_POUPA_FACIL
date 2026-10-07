using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.RateLimiting;
using API_POUPA_FACIL.Configuration;
using API_POUPA_FACIL.Context;
using API_POUPA_FACIL.Infrastructure;
using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Repository;
using API_POUPA_FACIL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace API_POUPA_FACIL.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPoupaFacil(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateOnStart();
        services.AddSingleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => ConfigureJwtBearer(options, configuration, environment));

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        var connectionString = DatabaseConnection.Resolve(configuration, environment);
        services.AddDbContext<BaseContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<IEmpresaService, EmpresaService>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problem = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Requisição inválida",
                        Instance = context.HttpContext.Request.Path
                    };
                    problem.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

                    return new BadRequestObjectResult(problem)
                    {
                        ContentTypes = { "application/problem+json" }
                    };
                };
            });

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddPolicy("login", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 10,
                        Window = TimeSpan.FromMinutes(1),
                        QueueLimit = 0
                    }));

            options.OnRejected = async (context, _) =>
            {
                if (context.HttpContext.Response.HasStarted)
                    return;

                await Results.Problem(
                        title: "Muitas tentativas",
                        detail: "Aguarde antes de tentar entrar novamente.",
                        statusCode: StatusCodes.Status429TooManyRequests,
                        instance: context.HttpContext.Request.Path)
                    .ExecuteAsync(context.HttpContext);
            };
        });

        services.AddHsts(options =>
        {
            options.MaxAge = TimeSpan.FromDays(180);
            options.IncludeSubDomains = false;
        });

        if (!environment.IsDevelopment())
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                // The platform proxy (Render) is not a known loopback address.
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        if (environment.IsDevelopment())
            services.AddDevelopmentSwagger();

        return services;
    }

    private static void ConfigureJwtBearer(
        JwtBearerOptions options,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
        var keyBytes = Encoding.UTF8.GetBytes(jwt.Key);

        options.RequireHttpsMetadata = !environment.IsDevelopment();
        options.SaveToken = false;
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            NameClaimType = JwtRegisteredClaimNames.Sub
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                if (context.Response.HasStarted)
                    return;

                await Results.Problem(
                        title: "Não autenticado",
                        detail: "Informe um token JWT válido.",
                        statusCode: StatusCodes.Status401Unauthorized,
                        instance: context.Request.Path)
                    .ExecuteAsync(context.HttpContext);
            },
            OnForbidden = async context =>
            {
                if (context.Response.HasStarted)
                    return;

                await Results.Problem(
                        title: "Acesso negado",
                        detail: "Você não tem permissão para esta operação.",
                        statusCode: StatusCodes.Status403Forbidden,
                        instance: context.Request.Path)
                    .ExecuteAsync(context.HttpContext);
            }
        };
    }

    private static void AddDevelopmentSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Poupa Fácil",
                Version = "v1"
            });

            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Informe o token JWT retornado pelo login."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });
        });
    }
}
