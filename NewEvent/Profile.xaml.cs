using NewEvent.Support;
using Newtonsoft.Json;
using System.Xml.Schema;

namespace NewEvent;

public partial class Profile : ContentPage
{
    private User user=new User();
    public Profile()
    {
        InitializeComponent();

        // Вивантажуємо нинішнього користувача
        string userJson = Preferences.Get("CurrentUser", null);
        user = JsonConvert.DeserializeObject<User>(userJson);

        var image = new Image()
        {
            Source = "profilephoto.jfif",
            Margin = new Thickness(0, 5, 0, 0)
        };

        // Додаємо мітку з ім'ям користувача
        var label = new Label()
        {
            Text = $"User name: {user.Nickname}",
            FontSize = 20,
            TextColor = Color.FromArgb("#FFFFFF"),
            Margin = new Thickness(0, 20, 0, 0)
        };

        // Додаємо кнопку для відкриття налаштувань
        var settingsButton = new Button()
        {
            Text = "Settings",
            TextColor = Color.FromArgb("#FFFFFF"),
            BackgroundColor = Color.FromArgb("#023402"),
            Margin = new Thickness(0, 20, 0, 0)
        };

        // Додаємо обробник події для кнопки налаштувань
        settingsButton.Clicked += (sender, e) =>
        {
            Navigation.PushModalAsync(new Settings());
        };

        var labelOfEvents = new Label()
        {
            Text = "Your events:",
            FontSize = 20,
            Margin = new Thickness(0, 20, 0, 0),
            TextColor = Color.FromArgb("#FFFFFF")
        };

        // Створюємо стековий макет і додаємо до нього мітку та кнопку
        var stackLayout = new StackLayout()
        {
            VerticalOptions = LayoutOptions.StartAndExpand,
            Margin = new Thickness(20)
        };
        stackLayout.Children.Add(image);
        stackLayout.Children.Add(label);
        stackLayout.Children.Add(settingsButton);
        stackLayout.Children.Add(labelOfEvents);

        // Вивантажуємо список подій користувача
        string events = Preferences.Get("Events", null);
        List<Event> eventList = JsonConvert.DeserializeObject<List<Event>>(events);

        // Додаємо кнопки для кожної події користувача
        foreach (var eventItem in eventList)
        {
            if (eventItem.Email == user.Email)
            {
                var button = new Button()
                {
                    Text = eventItem.Name,
                    TextColor = Color.FromArgb("#FFFFFF"),
                    BackgroundColor = Color.FromArgb("#023402"),
                    Margin = new Thickness(0, 5, 0, 0)
                };

                // Додаємо обробник події для кнопки
                button.Clicked += (sender, e) =>
                {
                    Navigation.PushModalAsync(new EventManager(eventItem, user));
                };

                // Додаємо кнопку до стекового макету
                stackLayout.Children.Add(button);
            }
        }

        // Встановлюємо стековий макет як вміст сторінки
        Content = stackLayout;
    }

}