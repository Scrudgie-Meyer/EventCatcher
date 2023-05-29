using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MySql.Data.MySqlClient;
using NewEvent.Support;
using Newtonsoft.Json;
using System.Globalization;

namespace NewEvent;

public partial class EventManager : ContentPage
{
    private Event ewent=new Event();
    private User user=new User();
    private bool IsPrivate = false;
    private string locationToSQL;
    public EventManager(Event ewent, User user)
	{
        InitializeComponent();

        this.ewent = ewent;
        this.user = user;
        locationToSQL = ewent.Location;

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
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (Name?.Text?.Length < 3 || Description?.Text?.Length < 3|| locationToSQL==null) await DisplayAlert("Error", "Fill all fields!", "OK");
        else
        {
            //З'єднання з базою даних
            MySqlConnection connection = CustomSQL.Connection();
            MySqlCommand command = new MySqlCommand($"SELECT * FROM Events WHERE Name=@Name AND Location=@Location AND Email=@Email", connection);
            command.Parameters.AddWithValue("@Name", ewent.Name);
            command.Parameters.AddWithValue("@Location", ewent.Location);
            command.Parameters.AddWithValue("@Email", ewent.Email);
            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read()) // перевіряємо, чи є результати запиту
            {
                reader.Close();
                MySqlCommand updateCommand = new MySqlCommand($"UPDATE Events SET Name=@Name, Date=@Date, Location=@Location, Description=@Description, IsPrivate=@IsPrivate", connection);
                updateCommand.Parameters.AddWithValue("@Name", Name.Text);
                updateCommand.Parameters.AddWithValue("@Date", Date.Date);
                updateCommand.Parameters.AddWithValue("@Location", locationToSQL);
                updateCommand.Parameters.AddWithValue("@Description", Description.Text);
                updateCommand.Parameters.AddWithValue("@IsPrivate", IsPrivate);

                updateCommand.ExecuteNonQuery();

            }
        }
        
    }
    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        //З'єднання з базою даних
        MySqlConnection connection = CustomSQL.Connection();
        MySqlCommand command = new MySqlCommand($"DELETE FROM Events WHERE Id = {ewent.Id}", connection);
        command.ExecuteReader();
        await Navigation.PopModalAsync();
    }
}