using ShamirApp.Services;

namespace ShamirApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Index}");

            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddFile(pathFormat: "log.txt", minimumLevel: LogLevel.Debug, fileSizeLimitBytes: 4294967296);
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var sqlClient = NpgsqlClient.GetInstance(connectionString, loggerFactory.CreateLogger<NpgsqlClient>());
            sqlClient.Init();

            app.Run();
        }
    }
}