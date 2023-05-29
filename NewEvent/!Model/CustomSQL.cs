using MySql.Data.MySqlClient;
using Newtonsoft.Json;


namespace NewEvent.Support
{
    internal static class CustomSQL
    {
        public static MySqlConnection Connection()
        {
            //З'єднання з базою даних
            string Conn = "Server=sql7.freemysqlhosting.net;Port=3306;Database=sql7611982;Uid=sql7611982;Pwd=Aegl446cFD;";
            MySqlConnection connection = new MySqlConnection(Conn);
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
            return connection;
        }
        public static User GetUserLogin(MySqlConnection connection,string Email, string Password)
        {
            User user= new User();
            //Пошук значення
            MySqlCommand command = new MySqlCommand($"SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection);
            command.Parameters.AddWithValue("@Email", Email);

            string pass = CustomHash.HashPassword(Password);
            command.Parameters.AddWithValue("@Password", pass);

            //Вивантажуємо користувача та зберігаємо 
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    user.Id = reader.GetInt32("Id");
                    user.Email = reader.GetString("Email");
                    user.Nickname = reader.GetString("Nickname");
                    user.Password = reader.GetString("Password");
                }
                string userJson = JsonConvert.SerializeObject(user);
                Preferences.Set("CurrentUser", userJson);
            }
            return user;
        }
        public static User GetUserProfile(MySqlConnection connection, string Email, string Password)
        {
            User user = new User();
            //Пошук значення
            MySqlCommand command = new MySqlCommand($"SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection);
            command.Parameters.AddWithValue("@Email", Email);

            command.Parameters.AddWithValue("@Password", Password);

            //Вивантажуємо користувача та зберігаємо 
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    user.Id = reader.GetInt32("Id");
                    user.Email = reader.GetString("Email");
                    user.Nickname = reader.GetString("Nickname");
                    user.Password = reader.GetString("Password");
                }
            }
            return user;
        }
        public static List<Event> GetEvents(MySqlConnection connection) 
        {
            List<Event> eventList = new List<Event>();

            //Запит на отримання всіх івентів з таблиці Events
            string query = "SELECT * FROM Events";

            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            eventList.Clear();
            while (reader.Read())
            {
                Event ewent = new Event();
                ewent.Id = reader.GetInt32("Id");
                ewent.Name = reader.GetString("Name");
                ewent.Date = reader.GetDateTime("Date");
                ewent.Location = reader.GetString("Location");
                ewent.Description = reader.GetString("Description");
                ewent.IsPrivate = reader.GetBoolean("IsPrivate");
                ewent.Email = reader.GetString("Email");
                ewent.UniqueCode = reader.GetString("UniqueCode");
                ewent.Participants = reader.GetString("Participants");
                eventList.Add(ewent);
            }
            reader.Close();
            return eventList;   
        }
        
    }
}
