using API_POUPA_FACIL.Context;
using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;

namespace API_POUPA_FACIL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //VERS�O PARA QUANTO IMPLEMENTARMOS UMA VARIAVEL DE AMBIENTE
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? 
                throw new InvalidOperationException("JWT_KEY not found locally"));

            // var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

           string connectionString;

            if (builder.Environment.IsDevelopment())
                connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            else
            {      
                var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                if (databaseUrl == null)
                {
                    throw new InvalidOperationException("DATABASE_URL environment variable is not set.");
                }

                var databaseUri = new Uri(databaseUrl);
                var userInfo = databaseUri.UserInfo.Split(':');

                var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                {
                    Host = databaseUri.Host,
                    Port = databaseUri.IsDefaultPort ? 5432 : databaseUri.Port, 
                    Username = userInfo[0],
                    Password = userInfo.Length > 1 ? userInfo[1] : string.Empty,
                    Database = databaseUri.LocalPath.TrimStart('/')
                };

                connectionString = connectionStringBuilder.ToString();
            }

            builder.Services.AddDbContext<BaseContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddControllers();

            builder.Services.AddScoped<IUsuarios, UsuarioRepository>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BaseContext>();
                dbContext.Database.Migrate();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();//http://localhost:5159/swagger
            }

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}