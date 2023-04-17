using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MySql.Data.MySqlClient;
using NewEvent.Support;
using Newtonsoft.Json;
using System.Globalization;

namespace NewEvent;

public partial class EventManager : ContentPage
{
    private Event ewent=null;
    private User user=null;
    private bool IsPrivate = false;
    private string locationToSQL;
    public EventManager(Event ewent, User user)
	{
        InitializeComponent();

        this.ewent = ewent;
        this.user = user;

        Name.Text = ewent.Name;
        Date.Date = ewent.Date;


        map.GestureRecognizers.Clear();
        string input = ewent.Location;
        string[] parts = input.Split(' ');
        double x = double.Parse(parts[0].Split('=')[1], CultureInfo.InvariantCulture);
        string yString = parts[1].Split('=')[1];
        if (yString.EndsWith("}"))
        {
            yString = yString.Substring(0, yString.Length - 1); // Remove the last character
        }
        double y = double.Parse(yString, CultureInfo.InvariantCulture);
        var location = new Location(y, x);
        // Встановлюємо пін на карту
        var pin = new Pin
        {
            Type = PinType.Generic,
            Location = location,
            Label = "Event location"
        };
        map.Pins.Add(pin);
        var mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(3));
        map.MoveToRegion(mapSpan);
        // Додаємо обробник події для натискання на карту
        map.MapClicked += (sender, e) =>
        {
            pin = new Pin
            {
                Type = PinType.Generic,
                Location = e.Location,
                Label = "Event location"
            };

            Point point = new Point(e.Location.Longitude, e.Location.Latitude);
            locationToSQL = Convert.ToString(point);
            map.Pins.Clear();
            map.Pins.Add(pin);
        };

        Description.Text=ewent.Description;
        UniqueCode.Text=$"Event UniqueCode: {ewent.UniqueCode}";
    }
    private async void OnReturnClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    private void OnPrivacyClicked(object sender, EventArgs e)
    {
        if (IsPrivate == false)
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
    private void OnSaveClicked(object sender, EventArgs e)
    {
        //З'єднання з базою даних
        string Conn = "Server=sql7.freemysqlhosting.net;Port=3306;Database=sql7611982;Uid=sql7611982;Pwd=Aegl446cFD;";
        MySqlConnection connection = new MySqlConnection(Conn);
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }
        MySqlCommand command = new MySqlCommand($"SELECT * FROM Events WHERE Name=@Name AND Location=@Location AND Email=@Email", connection);
        command.Parameters.AddWithValue("@Name", ewent.Name);
        command.Parameters.AddWithValue("@Location", ewent.Location);
        command.Parameters.AddWithValue("@Email", ewent.Email);
        MySqlDataReader reader = command.ExecuteReader();

        if (reader.Read()) // перевіряємо, чи є результати запиту
        {
            reader.Close();
            MySqlCommand updateCommand = new MySqlCommand($"UPDATE Events SET Name=@Name, Date=@Date, Location=@Location, Description=@Description, IsPrivate=@IsPrivate WHERE Email=@Email", connection);
            updateCommand.Parameters.AddWithValue("@Name", ewent.Name);
            updateCommand.Parameters.AddWithValue("@Date", ewent.Date);
            updateCommand.Parameters.AddWithValue("@Location", ewent.Location);
            updateCommand.Parameters.AddWithValue("@Description", ewent.Description);
            updateCommand.Parameters.AddWithValue("@IsPrivate", ewent.IsPrivate);
            updateCommand.Parameters.AddWithValue("@Email", ewent.Email);

            updateCommand.ExecuteNonQuery();

        }
    }
}