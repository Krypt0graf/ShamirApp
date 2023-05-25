using Npgsql;
using System.Text;

namespace ShamirApp.Helpers
{
    public static class PostgresExceptionHandler
    {
        public static void Handle(PostgresException ex, Dictionary<string, string> info = null)
        {
            //var logger = ExtendedLogger.ExtendedLogger.GetInstance();
            switch (ex.SqlState)
            {
                case PostgresErrorCodes.UniqueViolation:

                    var message = new StringBuilder();
                    message.Append($"{ex.MessageText}, table='{ex.TableName}'; ");
                    if(info is not null)
                        message.Append(string.Join(" ,", info.Select(i => $"{i.Key}='{i.Value}'")));

                    /*logger.Log(
                        LogLevel.Error,
                        message.ToString(),
                        new ExtendedLogger.LogInfo("NPostgresExceptionHandler"));*/
                    var msg = message.ToString();
                    Console.WriteLine(msg);
                    break;
                default:
                    throw ex;
            }

        }
    }
}
