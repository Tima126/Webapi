using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DataAccess.Wrapper;
using Domain.Interfaces;
using BusinessLogic.Services;
using Domain.interfaces;
using DataAccess.Repository;
using Domain.Repository;
using Domain.Models;

namespace webApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



           
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<IDicountServices, DiscountService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<INotificationTypeService, NotificationTypeService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
            builder.Services.AddScoped<IOrderStatusService, OrderStatusService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<ISupplierProductService, SupplierProductService>();
            builder.Services.AddScoped<IWishlistService, WishlistService>();






            builder.Services.AddDbContext<FlowersStoreContext>(
                optionsAction:options => options.UseSqlServer(
                    connectionString: "Server=TIMA;Database=Flowers_store;Trusted_Connection=True;TrustServerCertificate=True;"));





            builder.Services.AddCors(o => o.AddPolicy("MyPolicy", bulder =>
            {
                bulder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();

            }));




            




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