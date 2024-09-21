using Microsoft.EntityFrameworkCore;

using Microsoft.OpenApi.Models;
using DataAccess.Models;

namespace webApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);




            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", bulder =>
            {
                bulder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();

            }));







            builder.Services.AddDbContext<FlowersStoreContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddControllers();

            // Настройка Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("MyPolicy");
            app.MapControllers();

            app.Run();
        }
    }
}