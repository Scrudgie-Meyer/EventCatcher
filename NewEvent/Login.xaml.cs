using MySql.Data.MySqlClient;
using NewEvent.Support;
using Newtonsoft.Json;

namespace NewEvent;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();

        //З'єднання з базою даних
        string Conn = "Server=sql7.freemysqlhosting.net;Port=3306;Database=sql7611982;Uid=sql7611982;Pwd=Aegl446cFD;";
        MySqlConnection connection = new MySqlConnection(Conn);
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }
        //Запит на отримання всіх івентів з таблиці Events
        string query = "SELECT * FROM Events";

        MySqlCommand command = new MySqlCommand(query, connection);
        MySqlDataReader reader = command.ExecuteReader();
        List<Event> eventList = new List<Event>();
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
            eventList.Add(ewent);
        }
        reader.Close();
        string events = JsonConvert.SerializeObject(eventList);
        Preferences.Set("Events", events);
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        //З'єднання з базою даних
        string Conn = "Server=sql7.freemysqlhosting.net;Port=3306;Database=sql7611982;Uid=sql7611982;Pwd=Aegl446cFD;";
        MySqlConnection connection = new MySqlConnection(Conn);
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }

        if(Email.Text==null||Password.Text==null)
        {
            await DisplayAlert("Помилка", "Заповність усі поля!", "OK");
        }
        //Пошук значення
        MySqlCommand command = new MySqlCommand($"SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection);
        command.Parameters.AddWithValue("@Email", Email.Text);

        string pass = CustomHash.HashPassword(Password.Text);
        command.Parameters.AddWithValue("@Password", pass);

        //Вивантажуємо користувача та зберігаємо 
        User user = new User();
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
        string userJson = JsonConvert.SerializeObject(user);
        Preferences.Set("CurrentUser", userJson);

        //Перевіряємо на відповідність
        if (Email.Text == user.Email && pass == user.Password)
        {
            await Navigation.PushModalAsync(new AppShell());
            Navigation.RemovePage(this);
        }
        else await DisplayAlert("Помилка", "Внесено некоретнi данi!", "OK");
    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Navigate to the registration page

        await Navigation.PushModalAsync(new Registration());
        Navigation.RemovePage(this);
    }
}