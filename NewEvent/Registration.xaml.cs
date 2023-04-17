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
        //З'єднання з базою даних
        string Conn = "Server=sql7.freemysqlhosting.net;Port=3306;Database=sql7611982;Uid=sql7611982;Pwd=Aegl446cFD;";
        MySqlConnection connection = new MySqlConnection(Conn);
        if(connection.State==System.Data.ConnectionState.Closed) 
        {
            connection.Open();
        }
        

        //Перевірка на відповідність паролю
        if(Email.Text==null||Password.Text==null||DubPassword.Text==null)
        {
            await DisplayAlert("Помилка", "Заповність усі поля!", "OK");
        }
        else if (Password.Text==DubPassword.Text && Password.Text.Length>=8 && RegistrationResources.IsEmailValid(Email.Text)) 
        {
            //Збереження даних
            MySqlCommand command = new MySqlCommand($"INSERT INTO Users (Email, Nickname, Password) VALUES (@Email, @Nickname, @Password)", connection);

            command.Parameters.AddWithValue("@Email", Email.Text);
            command.Parameters.AddWithValue("@Nickname", Nickname.Text);

            //Хешування
            string pass = CustomHash.HashPassword(Password.Text);
            command.Parameters.AddWithValue("@Password", pass);

            command.ExecuteNonQuery();

            //Повернутися на сторінку логіна
            await Navigation.PushModalAsync(new Login());
            Navigation.RemovePage(this);
        }
        else await DisplayAlert("Помилка", "Внесено некоретнi данi!", "OK");

    }
}