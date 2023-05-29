using Microsoft.Maui.Controls;
using MySql.Data.MySqlClient;
using NewEvent.Support;
using Newtonsoft.Json;

namespace NewEvent;

public partial class Settings : ContentPage
{
    private User user = new User();
    public Settings()
	{
		InitializeComponent();

        string userJson = Preferences.Get("CurrentUser", null);
        user = JsonConvert.DeserializeObject<User>(userJson);

        ChangeNickname.Text = user.Nickname;
    }
    private async void OnReturnClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        //�'������� � ����� �����
        MySqlConnection connection = CustomSQL.Connection();
        if (ChangeNickname.Text==user.Nickname&&OldPassword.Text!=null&&NewPassword.Text!=null)
        {

            //����� ��������
            MySqlCommand command = new MySqlCommand($"SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);

            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) // ����������, �� � ���������� ������
            {
                reader.Close();
                MySqlCommand updateCommand = new MySqlCommand($"UPDATE Users SET Password = @NewPassword WHERE Email = @Email AND Password = @Password", connection);
                updateCommand.Parameters.AddWithValue("@Email", user.Email);
                string pass1 = CustomHash.HashPassword(OldPassword.Text);
                updateCommand.Parameters.AddWithValue("@Password", pass1);
                string pass2 = CustomHash.HashPassword(NewPassword.Text);
                updateCommand.Parameters.AddWithValue("@NewPassword",pass2);

                updateCommand.ExecuteNonQuery();

            }
        }
        else if(ChangeNickname.Text != user.Nickname && OldPassword.Text == null && NewPassword.Text == null)
        {
            //����� ��������
            MySqlCommand command = new MySqlCommand($"SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);

            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) // ����������, �� � ���������� ������
            {
                reader.Close();
                // ��������� �������� ���� "Password" �� "Nickname" � ����� �����
                MySqlCommand updateCommand = new MySqlCommand($"UPDATE Users SET Nickname = @NewNickname WHERE Email = @Email AND Password = @Password", connection);
                updateCommand.Parameters.AddWithValue("@Email", user.Email);
                updateCommand.Parameters.AddWithValue("@Password", user.Password);
                updateCommand.Parameters.AddWithValue("@NewNickname", ChangeNickname.Text);

                updateCommand.ExecuteNonQuery();
            }
        }
        else if (ChangeNickname.Text != user.Nickname && OldPassword.Text != null && NewPassword.Text != null)
        {
            //����� ��������
            MySqlCommand command = new MySqlCommand($"SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);

            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) // ����������, �� � ���������� ������
            {
                reader.Close();
                // ��������� �������� ���� "Password" �� "Nickname" � ����� �����
                MySqlCommand updateCommand = new MySqlCommand($"UPDATE Users SET Password = @NewPassword, Nickname = @NewNickname WHERE Email = @Email AND Password = @Password", connection);
                updateCommand.Parameters.AddWithValue("@Email", user.Email);
                string pass1 = CustomHash.HashPassword(OldPassword.Text);
                updateCommand.Parameters.AddWithValue("@Password", pass1);
                string pass2 = CustomHash.HashPassword(NewPassword.Text);
                updateCommand.Parameters.AddWithValue("@NewPassword", pass2);
                updateCommand.Parameters.AddWithValue("@NewNickname", ChangeNickname.Text);

                updateCommand.ExecuteNonQuery();
            }
        }
        else await DisplayAlert("�������", "������� ��������i ���i!", "OK");

        //������ �������� User
        MySqlCommand commandReload = new MySqlCommand($"SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection);
        commandReload.Parameters.AddWithValue("@Email", user.Email);
        commandReload.Parameters.AddWithValue("@Password", user.Password);
        using (MySqlDataReader reader = commandReload.ExecuteReader())
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
    }
}