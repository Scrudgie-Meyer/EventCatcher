using MySql.Data.MySqlClient;
using NewEvent.Support;
using Newtonsoft.Json;

namespace NewEvent;

public partial class Login : ContentPage
{
    private User user = null;
    private List<Event> eventList = new List<Event>();
    public Login()
	{
		InitializeComponent();

        //�'������� � ����� �����
        MySqlConnection connection = CustomSQL.Connection();

        //����������� ������ ������
        eventList = CustomSQL.GetEvents(connection);

        string events = JsonConvert.SerializeObject(eventList);
        Preferences.Set("Events", events);
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        //�'������� � ����� �����
        MySqlConnection connection = CustomSQL.Connection();

        if (Email.Text==null||Password.Text==null)
        {
            await DisplayAlert("Error", "Fill all fields!", "OK");
        }
        else
        {
            //����� ��������
            user = CustomSQL.GetUserLogin(connection, Email.Text, Password.Text);

            //���������� �� ����������
            string pass = CustomHash.HashPassword(Password.Text);
            if (Email.Text == user.Email && pass == user.Password)
            {
                await Navigation.PushModalAsync(new AppShell());
                Navigation.RemovePage(this);
            }
            else await DisplayAlert("Error", "Entered incorrect data!", "OK");
        }
    }
    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Navigate to the registration page
        await Navigation.PushModalAsync(new Registration());
        Navigation.RemovePage(this);
    }
}