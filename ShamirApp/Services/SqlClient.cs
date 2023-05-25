using Npgsql;
using Serilog;
using ShamirApp.Helpers;
using ShamirApp.Models.Admin;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ShamirApp.Services
{
    public class NpgsqlClient
    {
        private static NpgsqlClient? Instance;
        private string? _connectionString;
        ILogger<NpgsqlClient>? _logger;
        private NpgsqlClient() { }

        public static NpgsqlClient GetInstance(string? connectionString = null, ILogger<NpgsqlClient>? logger = null)
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
        
        /// <summary>
        /// Проверка базы
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void Init()
        {
            if (_connectionString is null)
                throw new ArgumentException(_connectionString);

            // Проверка подключения к базе
            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();

            // Сделаем запрос всех таблиц
            _logger.LogInformation("Проверка таблиц в базе данных");

            var tables = new List<string>();
            var reader = ExecuteReader(select_all_tables, nc);
            while (reader.Read())
                tables.Add(reader.GetString("table_name"));
            reader.Close();

            _logger.LogInformation($"Таблиц найдено: {tables.Count}");

            // Если какие то отсутствуют - создадим
            if (!tables.Contains("users"))
            {
                _logger.LogInformation("Создание таблицы 'users'");
                ExecuteNonQuery(create_table_users, nc);
            }
            if (!tables.Contains("forms"))
            {
                _logger.LogInformation("Создание таблицы 'forms'");
                ExecuteNonQuery(create_table_forms, nc);
            }
            if (!tables.Contains("questions"))
            {
                _logger.LogInformation("Создание таблицы 'questions'");
                ExecuteNonQuery(create_table_questions, nc);
            }
        }

        #region [ADMIN USERS]
        /// <summary>
        /// Добавляет нового пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="fio">ФИО</param>
        /// <param name="isAdmin">Роль</param>
        /// <returns>id нового пользователя</returns>
        public int AddNewUser(string login, string password, string fio, bool isAdmin = false)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return 0;
            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();
            var sql = add_new_user
                .Replace("$login", login)
                .Replace("$password", password)
                .Replace("$fio", fio)
                .Replace("$isAdmin", isAdmin ? "1" : "0")
                .Replace("$token", Guid.NewGuid().ToString());
            try
            {
                var reader = ExecuteReader(sql, nc);
                if (reader.Read())
                    return reader.GetInt32("id");
                return 0;
            }
            catch (PostgresException ex)
            {
                _logger?.LogError(ex.MessageText);
            }   
            return -1;
        }

        public int EditUser(int id, string login, string password, string fio)
        {
            if (id <= 0 || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return 0;
            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();
            var sql = edit_user
                .Replace("$id", id.ToString())
                .Replace("$login", login)
                .Replace("$password", password)
                .Replace("$fio", fio);
            try
            {
                return ExecuteNonQuery(sql, nc);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return -1;
            }
        }

        public int DeleteUser(int id)
        {
            if (id <= 0)
                return 0;
            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();
            var sql = delete_user
                .Replace("$id", id.ToString());
            try
            {
                return ExecuteNonQuery(sql, nc);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return -1;
            }
        }

        public (bool exist, bool isAdmin, string token) GetUserFromLogin(string login, string password)
        {
            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();
            var sql = get_user_from_login_and_password
                .Replace("$login", login)
                .Replace("$password", password);
            var reader = ExecuteReader(sql, nc);
            if (reader.Read())
                return (true, reader.GetBoolean("isAdmin"), reader.GetString("token"));
            return (false, false, "");
        }

        public (bool exist, bool isAdmin) GetUserFromToken(string? token)
        {
            if (token == null)
                return (false, false);

            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();
            var sql = get_user_from_token
                .Replace("$token", token);
            var reader = ExecuteReader(sql, nc);
            if (reader.Read())
                return (true, reader.GetBoolean("isAdmin"));
            return (false, false);
        }

        public List<UserView> GetUsers()
        {
            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();
            var list = new List<UserView>();
            var reader = ExecuteReader(get_users, nc);
            while (reader.Read())
                list.Add(new UserView()
                {
                    Id = reader.GetInt32("id"),
                    Login = reader.GetString("login"),
                    Password = reader.GetString("password"),
                    FIO = reader.GetString("fio")
                });
            return list;
        }
        #endregion
        #region [ADMIN FORMS]
        public int AddNewForm(string title)
        {
            if (string.IsNullOrEmpty(title))
                return 0;
            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();
            var sql = add_new_form
                .Replace("$title", title);
            try
            {
                var reader = ExecuteReader(sql, nc);
                if (reader.Read())
                    return reader.GetInt32("id");
                return 0;
            }
            catch (PostgresException ex)
            {
                _logger?.LogError(ex.MessageText);
            }
            return 0;
        }
        public int AddNewQuestions(string[] texts, int idform)
        {
            if (texts.Length == 0 || idform <= 0)
                return 0;
            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();

            var sql = add_new_questions
                .Replace("$values", string.Join(", ", texts.Select(text => $"('{text}', '{idform}')")));
            try
            {
                return ExecuteNonQuery(sql, nc);
            }
            catch (PostgresException ex)
            {
                _logger?.LogError(ex.MessageText);
            }
            return 0;
        }

        public List<(int id, string title)> GetAllForms()
        {
            var forms = new List<(int id, string title)>();

            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();

            var sql = get_all_forms;
            try
            {
                var reader = ExecuteReader(sql, nc);
                while (reader.Read())
                {
                    forms.Add((reader.GetInt32("id"), reader.GetString("title")));
                }
            }
            catch (PostgresException ex)
            {
                _logger?.LogError(ex.MessageText);
            }
            return forms;
        }

        public List<(int id, string text)> GetQuestions(int idform)
        {
            var qs = new List<(int id, string text)>();

            using var nc = new NpgsqlConnection(_connectionString);
            nc.Open();

            var sql = get_questions
                .Replace("$idform", idform.ToString());
            try
            {
                var reader = ExecuteReader(sql, nc);
                while (reader.Read())
                {
                    qs.Add((reader.GetInt32("id"), reader.GetString("text")));
                }
            }
            catch (PostgresException ex)
            {
                _logger?.LogError(ex.MessageText);
            }
            return qs;
        }
        #endregion
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
        private static int ExecuteNonQuery(string query, NpgsqlConnection nc)
        {
            NpgsqlCommand command = new NpgsqlCommand(query, nc);
            return command.ExecuteNonQuery();
        }

        #endregion
        #region [scripts]
        private const string select_all_tables =
            "select table_name from information_schema.tables " +
            "where table_schema not in ('information_schema','pg_catalog');";

        private const string create_table_users =
            "create table users (" +
            "id serial primary key," +
            "login varchar(20) not null," +
            "password varchar(20) not null," +
            "fio varchar(50)," +
            "isAdmin bool," +
            "token varchar(36));" +
            "CREATE UNIQUE INDEX login_password ON users (login, password);";

        private const string create_table_forms =
            "create table forms(" +
            "id serial primary key," +
            "title VARCHAR(200) not null);";

        private const string create_table_questions =
            "create table questions(" +
            "id serial primary key," +
            "text VARCHAR(200) not null," +
            "idForm integer references forms (id));";

        private static string get_users =
            "select u.id, u.login, u.password, u.fio from users u where u.isAdmin = false order by id desc";

        private const string add_new_user =
            "insert into users (login, password, fio, isAdmin, token)" +
            "values ('$login', '$password', '$fio', '$isAdmin', '$token') returning id;";

        private const string edit_user =
            "update users set login = '$login', password = '$password', fio = '$fio' where id = $id";

        private const string delete_user =
            "delete from users where id = $id";

        private const string get_user_from_login_and_password =
            "select u.isadmin, u.token from users u where login = '$login' and password = '$password'";

        private const string get_user_from_token =
            "select u.isadmin from users u where token = '$token'";

        private const string add_new_form =
            "insert into forms (title) values ('$title') returning id;";

        private const string add_new_questions =
            "insert into questions (text, idform) values $values;";

        private const string get_all_forms =
            "select * from forms";

        public const string get_questions =
            "select * from questions where idform = '$idform'";
        #endregion
    }
}
