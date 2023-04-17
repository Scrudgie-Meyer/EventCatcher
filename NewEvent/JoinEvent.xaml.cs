using NewEvent.Support;
using Newtonsoft.Json;

namespace NewEvent;

public partial class JoinEvent : ContentPage
{
    Event ewent=null;
	public JoinEvent()
	{
		InitializeComponent();
        
    }
    private async void OnJoinClicked(object sender, EventArgs e)
    {
        string events = Preferences.Get("Events", null);
        List<Event> eventList = JsonConvert.DeserializeObject<List<Event>>(events);
        foreach (Event ev in eventList)
        {
            if (Pass.Text == ev.UniqueCode) { ewent = ev; }
        }
        if (ewent == null) { await DisplayAlert("Помилка", "Івент не знайдено!", "OK"); }
        await Navigation.PushModalAsync(new EventView(ewent));
    }
}