using Microsoft.Maui.ApplicationModel.Communication;
using MySql.Data.MySqlClient;
using NewEvent.Support;

namespace NewEvent;

public partial class Registration : ContentPage
{
	public Registration()
	{
		InitializeComponent();
	}
    private async void OnReturnClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Login());
        Navigation.RemovePage(this);
    }
    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        //�'������� � ����� �����
        string Conn = "Server=sql7.freemysqlhosting.net;Port=3306;Database=sql7611982;Uid=sql7611982;Pwd=Aegl446cFD;";
        MySqlConnection connection = new MySqlConnection(Conn);
        if(connection.State==System.Data.ConnectionState.Closed) 
        {
            connection.Open();
        }
        

        //�������� �� ���������� ������
        if(Email.Text==null||Password.Text==null||DubPassword.Text==null)
        {
            await DisplayAlert("�������", "��������� �� ����!", "OK");
        }
        else if (Password.Text==DubPassword.Text && Password.Text.Length>=8 && RegistrationResources.IsEmailValid(Email.Text)) 
        {
            //���������� �����
            MySqlCommand command = new MySqlCommand($"INSERT INTO Users (Email, Nickname, Password) VALUES (@Email, @Nickname, @Password)", connection);

            command.Parameters.AddWithValue("@Email", Email.Text);
            command.Parameters.AddWithValue("@Nickname", Nickname.Text);

            //���������
            string pass = CustomHash.HashPassword(Password.Text);
            command.Parameters.AddWithValue("@Password", pass);

            command.ExecuteNonQuery();

            //����������� �� ������� �����
            await Navigation.PushModalAsync(new Login());
            Navigation.RemovePage(this);
        }
        else await DisplayAlert("�������", "������� ��������i ���i!", "OK");

    }
}