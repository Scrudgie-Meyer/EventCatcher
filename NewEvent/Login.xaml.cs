using MySql.Data.MySqlClient;
using NewEvent.Support;

namespace NewEvent;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();

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

        //Пошук значення
        MySqlCommand command = new MySqlCommand($"SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection);
        command.Parameters.AddWithValue("@Email", Email.Text);

        string pass = CustomHash.HashPassword(Password.Text);
        command.Parameters.AddWithValue("@Password", pass);

        
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