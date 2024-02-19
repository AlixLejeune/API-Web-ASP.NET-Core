using Microsoft.EntityFrameworkCore;
using API_Web_ASP.NET_Core.Models.EntityFramework;

namespace API_Web_ASP.NET_Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // A ajouter pour mettre en place la dépendance et le contexte, sans quoi l'APi plante -> string de connexion dans appsettings.json
            builder.Services.AddDbContext<FilmRatingsDBContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("FilmsDBContext")));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}