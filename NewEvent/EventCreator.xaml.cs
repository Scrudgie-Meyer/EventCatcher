using MySql.Data.MySqlClient;
using NewEvent.Support;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Newtonsoft.Json;

namespace NewEvent;

public partial class EventCreator : ContentPage
{
    private User user = new User();
    private bool IsPrivate = false; 
    private string locationToSQL;
	public EventCreator()
	{
		InitializeComponent();

        string userJson = Preferences.Get("CurrentUser", null);
        user = JsonConvert.DeserializeObject<User>(userJson);

        map.GestureRecognizers.Clear();
        var location = new Location(50.4501, 30.5234);

        var mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(3));
        map.MoveToRegion(mapSpan);

        // Додаємо обробник події для натискання на карту
        map.MapClicked += (sender, e) =>
        {
            // Встановлюємо пін на карту
            var pin = new Pin
            {
                Type = PinType.Generic,
                Location = e.Location,
                Label="Event location"
            };
            Point point= new Point(e.Location.Longitude, e.Location.Latitude);
            locationToSQL =Convert.ToString(point);
            map.Pins.Clear();

            map.Pins.Add(pin);
        };
    }
    private void OnPrivacyClicked(object sender, EventArgs e)
    {
        if(IsPrivate==false) 
        {
            PrivatePress.Text = "Private";
            IsPrivate = true;
        }
        else
        {
            PrivatePress.Text = "Public";
            IsPrivate = false;
        }
    }
    private async void OnCreateClicked(object sender, EventArgs e)
    {
        //З'єднання з базою даних
        string Conn = "Server=sql7.freemysqlhosting.net;Port=3306;Database=sql7611982;Uid=sql7611982;Pwd=Aegl446cFD;";
        MySqlConnection connection = new MySqlConnection(Conn);
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }
        if (Name.Text == null || locationToSQL == null || Description.Text==null)
        {
            await DisplayAlert("Помилка", "Заповність усі поля!", "OK");
        }
        string uniqueCode = Guid.NewGuid().ToString().Substring(0, 8);
        //Збереження даних
        MySqlCommand command = new MySqlCommand($"INSERT INTO Events (Name, Date, Location, Description, IsPrivate, Email, UniqueCode) VALUES (@Name, @Date, @Location, @Description, @IsPrivate, @Email, @UniqueCode)", connection);
        command.Parameters.AddWithValue("@Name", Name.Text);
        command.Parameters.AddWithValue("@Date", Date.Date);
        command.Parameters.AddWithValue("@Location",locationToSQL);
        command.Parameters.AddWithValue("@Description", Description.Text);
        command.Parameters.AddWithValue("@IsPrivate", IsPrivate);
        command.Parameters.AddWithValue("@Email", user.Email);
        command.Parameters.AddWithValue("@UniqueCode", uniqueCode);
        command.ExecuteNonQuery();
    }
}