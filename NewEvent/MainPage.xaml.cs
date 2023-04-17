using NewEvent.Support;
using Newtonsoft.Json;

namespace NewEvent
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            string events = Preferences.Get("Events", null);
            List<Event> eventList = JsonConvert.DeserializeObject<List<Event>>(events);

            var label = new Label()
            {
                Text = "Popular public events",
                FontSize = 20,
                TextColor = Color.FromArgb("#FFFFFF"),
                Margin = new Thickness(0, 20, 0, 0)
            };
            var stackLayout = new StackLayout();
            stackLayout.Children.Add(label);
            foreach (var eventItem in eventList)
            {
                if(eventItem.IsPrivate==false)
                {
                    var button = new Button()
                    {
                        Text = eventItem.Name,
                        TextColor = Color.FromArgb("#FFFFFF"),
                        BackgroundColor = Color.FromArgb("#023402"),
                        Margin = new Thickness(0, 5, 0, 0)
                    };

                    // додаємо обробник події для кнопки
                    button.Clicked += (sender, e) => 
                    {
                        Navigation.PushModalAsync(new EventView(eventItem));
                    };

                    // додаємо кнопку до кореневого макету
                    stackLayout.Children.Add(button);
                }
                
            }
            Content = stackLayout;
        }
        


    }
}