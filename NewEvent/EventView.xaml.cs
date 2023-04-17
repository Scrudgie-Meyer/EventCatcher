using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using NewEvent.Support;
using System.Globalization;

namespace NewEvent;

public partial class EventView : ContentPage
{
	public EventView(Event ewent)
	{
		InitializeComponent();

        Name.Text = ewent.Name;
        Date.Text = ewent.Date.ToString("yyyy-MM-dd");

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
        UniqueCode.Text = $"UnigueCode: {ewent.UniqueCode}";
    }
    private async void OnReturnClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    private void OnJoinClicked(object sender, EventArgs e)
    {
        
    }
}