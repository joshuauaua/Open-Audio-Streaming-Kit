using HLS_Tool.Data;
using HLS_Tool.Repositories.TrackRepositories;
using HLS_Tool.Services.TrackServices;
using HLS_Tool.Services.UploadServices;
using Microsoft.EntityFrameworkCore;

namespace Hyper_Radio_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Database
            builder.Services.AddDbContext<HLSToolDbContext>(options =>
                options.UseSqlite(connectionString));

            // MVC with views (required for starting on Home/Index)
            builder.Services.AddControllersWithViews();

            // Dependency Injection services
            builder.Services.AddScoped<ITrackService, TrackService>();
            builder.Services.AddScoped<ITrackRepository, TrackRepository>(); // <-- missing one!
            builder.Services.Configure<BlobSettings>(builder.Configuration.GetSection("AzureBlob"));
            builder.Services.AddSingleton<BlobService>();
            builder.Services.AddSingleton<HlsConverterService>();

            // CORS configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                        "https://localhost:7110",
                        "http://localhost:5122",
                        "hyper-radio-streamer-gqeaffc3cucfhxb8.norwayeast-01.azurewebsites.net"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            var app = builder.Build();

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();        // required for CSS/JS/wwwroot
            app.UseRouting();
            app.UseCors("AllowFrontend");
            app.UseAuthorization();

            // 👉 Default route = Home/Index
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=TracksView}/{action=Index}/{id?}");

            // Optional if you also want API endpoints using attribute routing
            // app.MapControllers();

            app.Run();
        }
    }
}