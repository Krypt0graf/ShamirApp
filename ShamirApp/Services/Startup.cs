namespace ShamirApp.Services
{
    public static class Startup
    {
        public static void Init(WebApplicationBuilder builder)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(logger =>
            {
                logger.AddConsole();
                logger.AddFile(pathFormat: "LOG.txt", minimumLevel: LogLevel.Debug, fileSizeLimitBytes: 4294967296);
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            var sqlClient = NpgsqlClient.GetInstance(connectionString, loggerFactory.CreateLogger<NpgsqlClient>());
            sqlClient.Init();
        }
    }
}
