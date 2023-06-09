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

        // ������ �������� ��䳿 ��� ���������� �� �����
        map.MapClicked += (sender, e) =>
        {
            // ������������ �� �� �����
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
        //�'������� � ����� �����
        MySqlConnection connection=CustomSQL.Connection();
        if (Name.Text == null || locationToSQL == null || Description.Text==null)
        {
            await DisplayAlert("Error", "Fill all fields!", "OK");
        }
        else 
        {
            string uniqueCode = Guid.NewGuid().ToString().Substring(0, 8);
            //���������� �����
            MySqlCommand command = new MySqlCommand($"INSERT INTO Events (Name, Date, Location, Description, IsPrivate, Email, UniqueCode, Participants) VALUES (@Name, @Date, @Location, @Description, @IsPrivate, @Email, @UniqueCode, @Participants)", connection);
            command.Parameters.AddWithValue("@Name", Name.Text);
            command.Parameters.AddWithValue("@Date", Date.Date);
            command.Parameters.AddWithValue("@Location", locationToSQL);
            command.Parameters.AddWithValue("@Description", Description.Text);
            command.Parameters.AddWithValue("@IsPrivate", IsPrivate);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@UniqueCode", uniqueCode);
            command.Parameters.AddWithValue("@Participants", user.Email);

            command.ExecuteNonQuery();
        }
        
    }
}