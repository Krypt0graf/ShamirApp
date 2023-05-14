using Npgsql;
using System.Data;

namespace ShamirApp.Services
{
    public class NpgsqlClient
    {
        private static NpgsqlClient Instance;
        private string _connectionString;
        ILogger<NpgsqlClient> _logger;
        private NpgsqlClient(){ }

        public static NpgsqlClient GetInstance(string connectionString = null, ILogger<NpgsqlClient> logger = null)
        {
            if (Instance == null)
            {
                Instance = new NpgsqlClient()
                {
                    _connectionString = connectionString,
                    _logger = logger
                };
            }
            return Instance;
        }
        public void Init()
        {
            if (_connectionString is null)
                throw new ArgumentException(_connectionString);

            // Сделаем запрос всех таблиц
            var tables = new List<string>();
            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();
            var reader = ExecuteReader(select_all_tables, nc);
            while (reader.Read())
                tables.Add(reader.GetString("table_name"));
            reader.Close();

            // Если какие то отсутствуют - создадим
            if (!tables.Contains("users"))
            {
                _logger.LogInformation("Создание таблицы 'users'");
                ExecuteNonQuery(create_table_users, nc);
            }
        }
        #region [ExecuteQuery]

        /// <summary>
        /// Для SELECT запросов (с выборкой данных)
        /// </summary>
        /// <param name="query">SQL Запрос</param>
        /// <param name="nc">Подключение</param>
        /// <returns></returns>
        private static NpgsqlDataReader ExecuteReader(string query, NpgsqlConnection nc)
        {
            NpgsqlCommand command = new NpgsqlCommand(query, nc);
            return command.ExecuteReader();
        }

        /// <summary>
        /// Для запросов без выборки данных
        /// </summary>
        /// <param name="query">SQL Запрос</param>
        /// <param name="nc">Подключение</param>
        /// <returns>Количество обработанных строк</returns>
        private static void ExecuteNonQuery(string query, NpgsqlConnection nc)
        {
            NpgsqlCommand command = new NpgsqlCommand(query, nc);
            command.ExecuteNonQuery();
        }

        #endregion
        #region [scripts]
        private const string select_all_tables =
            $"SELECT table_name FROM information_schema.tables " +
            $"WHERE table_schema NOT IN ('information_schema','pg_catalog');";

        private const string create_table_users =
            $"CREATE TABLE users (" +
            $"id serial primary key," +
            $"login VARCHAR(20) not null," +
            $"password VARCHAR(20) not null," +
            $"fio VARCHAR(50)" +
            $");";

        #endregion
    }
}
