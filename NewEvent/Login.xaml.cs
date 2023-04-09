using MySql.Data.MySqlClient;

namespace NewEvent;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        //�'������� � ����� �����
        string Conn = "Server=sql7.freemysqlhosting.net;Port=3306;Database=sql7611982;Uid=sql7611982;Pwd=Aegl446cFD;";
        MySqlConnection connection = new MySqlConnection(Conn);
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }

        //����� ��������
        MySqlCommand command = new MySqlCommand($"SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection);
        command.Parameters.AddWithValue("@Email", Email.Text);
        command.Parameters.AddWithValue("@Password", Password.Text);

        
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
        
        if (Email.Text == user.Email && Password.Text == user.Password)
        {
            await Navigation.PushModalAsync(new AppShell());
            Navigation.RemovePage(this);
        } 

        Email.Text = "������� ����������i ���i!";
    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Navigate to the registration page

        await Navigation.PushModalAsync(new Registration());
        Navigation.RemovePage(this);
    }
}