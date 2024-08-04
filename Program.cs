using API_POUPA_FACIL.Interfaces;
using API_POUPA_FACIL.Repository;

namespace API_POUPA_FACIL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddScoped<IUsuarios, UsuarioRepository>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();//http://localhost:5159/swagger
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}