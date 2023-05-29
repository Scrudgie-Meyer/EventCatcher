using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using MySql.Data.MySqlClient;
using NewEvent.Support;
using Newtonsoft.Json;
using System.Globalization;

namespace NewEvent;

public partial class EventView : ContentPage
{
    private User user = new User();
    private Event ewent=new Event();
	public EventView(Event ewent)
	{
		InitializeComponent();
        this.ewent = ewent;

        // Вивантажуємо нинішнього користувача
        string userJson = Preferences.Get("CurrentUser", null);
        user = JsonConvert.DeserializeObject<User>(userJson);

        Name.Text = ewent.Name;
        Date.Text = "Date: "+ewent.Date.ToString("yyyy-MM-dd");

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


        Description.Text=ewent.Description;
        OrganisatorData.Text = $"Contacts: {ewent.Email}";
        UniqueCode.Text = $"UniqueCode: {ewent.UniqueCode}";
        List<string> participants = ewent.Participants.Split(',').ToList();
        Participants.Text= "Participants:\n" + String.Join("\n", participants);
        if (participants.Contains(user.Email)) Join.Text = "Leave";
    }
    private async void OnReturnClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    private async void OnJoinClicked(object sender, EventArgs e)
    {
        List<string> participants = ewent.Participants.Split(',').ToList();
        if (participants.Contains(user.Email)) Join.Text = "Leave";
        //З'єднання з базою даних
        MySqlConnection connection = CustomSQL.Connection();
        if (Join.Text == "Leave")
        {
            participants.Remove(user.Email);
            string newParticipants = string.Join(",", participants);
            MySqlCommand command = new MySqlCommand($"SELECT * FROM Events WHERE UniqueCode=@UniqueCode", connection);
            command.Parameters.AddWithValue("@UniqueCode", ewent.UniqueCode);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) // перевіряємо, чи є результати запиту
            {
                reader.Close();
                MySqlCommand updateCommand = new MySqlCommand($"UPDATE Events SET Participants=@Participants WHERE UniqueCode=@UniqueCode", connection);
                updateCommand.Parameters.AddWithValue("@Participants", newParticipants);
                updateCommand.Parameters.AddWithValue("@UniqueCode", ewent.UniqueCode);

                updateCommand.ExecuteNonQuery();
            }
            await DisplayAlert("Message", "You left event!", "OK");
            await Navigation.PopModalAsync();
        }
        else
        {
            participants.Add(user.Email);
            string newParticipants = string.Join(",", participants);
            MySqlCommand command = new MySqlCommand($"SELECT * FROM Events WHERE UniqueCode=@UniqueCode", connection);
            command.Parameters.AddWithValue("@UniqueCode", ewent.UniqueCode);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.Read()) // перевіряємо, чи є результати запиту
            {
                reader.Close();
                MySqlCommand updateCommand = new MySqlCommand($"UPDATE Events SET Participants=@Participants WHERE UniqueCode=@UniqueCode", connection);
                updateCommand.Parameters.AddWithValue("@Participants", newParticipants);
                updateCommand.Parameters.AddWithValue("@UniqueCode", ewent.UniqueCode);

                updateCommand.ExecuteNonQuery();
            }
            await DisplayAlert("Message", "You joined event!", "OK");
            await Navigation.PopModalAsync();
        }        
    }
}