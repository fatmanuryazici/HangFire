
using Hangfire;
using HangFireApp.Context;
using HangFireApp.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HangFire
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //connectionstring burada tan�ml�yoruz
            string connectionString = builder.Configuration.GetConnectionString("SqlServer");

            //Database ayar�m�z� yap�yoruz
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            //Hangfire � servise register ediyoruz ve sonra connectionstringi cag�r�yoruz
            builder.Services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(connectionString);
            });
            builder.Services.AddHangfireServer();

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

            app.UseHangfireDashboard(); //gorselle�tirmek i�in kullan�l�yor

            RecurringJob.AddOrUpdate("test-job", () => BackgroundTestServices.Test(),Cron.Minutely()); //hem senkron hem asenkron metotlar� �al�st�r�r.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
