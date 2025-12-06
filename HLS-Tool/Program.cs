using Hyper_Radio_API.Data;
using Hyper_Radio_API.Services.TrackServices;
using Hyper_Radio_API.Services.UploadServices;
using Microsoft.EntityFrameworkCore;

namespace Hyper_Radio_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddDbContext<HLSToolDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddScoped<IUploadService, UploadService>();

            
            builder.Services.Configure<AzureBlobSettings>(
                builder.Configuration.GetSection("AzureBlob"));
            builder.Services.AddSingleton<BlobService>();
            builder.Services.AddSingleton<HlsConverterService>();
            
            
            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSwaggerUI", policy =>
                {
                    policy.AllowAnyOrigin()    // or specify origins
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
                options.AddPolicy("AllowFrontend",
                  policy =>
                  {
                      policy
                          .WithOrigins("https://localhost:7110",
                                       "http://localhost:5122",
                                       "hyper-radio-streamer-gqeaffc3cucfhxb8.norwayeast-01.azurewebsites.net")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // optional, if cookies or auth are used
                  });
            });


            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseCors("AllowFrontend");

            app.UseAuthorization();
            
            app.MapControllers();
            app.Run();
        }
    }
}