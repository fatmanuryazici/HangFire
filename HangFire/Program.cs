
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

            //connectionstring burada tanýmlýyoruz
            string connectionString = builder.Configuration.GetConnectionString("SqlServer");

            //Database ayarýmýzý yapýyoruz
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            //Hangfire ý servise register ediyoruz ve sonra connectionstringi cagýrýyoruz
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

            app.UseHangfireDashboard(); //gorselleþtirmek için kullanýlýyor

            RecurringJob.AddOrUpdate("test-job", () => BackgroundTestServices.Test(),Cron.Minutely()); //hem senkron hem asenkron metotlarý çalýstýrýr.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
