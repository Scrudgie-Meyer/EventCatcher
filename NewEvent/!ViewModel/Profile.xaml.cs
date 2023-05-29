using MySql.Data.MySqlClient;
using NewEvent.Support;
using Newtonsoft.Json;
using System.Xml.Schema;

namespace NewEvent;

public partial class Profile : ContentPage
{
    private User user = new User();
    private List<Event> eventList = new List<Event>();

    public Profile()
    {
        InitializeComponent();

        // Вивантажуємо нинішнього користувача та івенти
        string userJson = Preferences.Get("CurrentUser", null);
        user = JsonConvert.DeserializeObject<User>(userJson);
        eventList = CustomSQL.GetEvents(CustomSQL.Connection());

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
            TextColor = Color.FromArgb("#000000"),
            Margin = new Thickness(20, 10, 10, 0)
        };

        var settingsButton = new Button()
        {
            Text = "Settings",
            TextColor = Color.FromArgb("#FFFFFF"),
            BackgroundColor = Color.FromArgb("#FF05A27F"),
            Margin = new Thickness(15, 10, 15, 0)
        };

        // Додаємо обробник події для кнопки налаштувань
        settingsButton.Clicked += (sender, e) =>
        {
            Navigation.PushModalAsync(new Settings());
        };

        var labelOfJoinedEvents = new Label()
        {
            Text = "Joined events:",
            FontSize = 20,
            Margin = new Thickness(20, 20, 10, 0),
            TextColor = Color.FromArgb("#000000")
        };

        var joinedEventsStackLayout = new StackLayout()
        {
            Margin = new Thickness(5)
        };

        foreach (var eventItem in eventList)
        {
            List<string> participants = eventItem.Participants.Split(',').ToList();
            if (participants.Contains(user.Email) && eventItem.Email!=user.Email)
            {
                var button = new Button()
                {
                    Text = eventItem.Name,
                    TextColor = Color.FromArgb("#FFFFFF"),
                    BackgroundColor = Color.FromArgb("#FF05A27F"),
                    Margin = new Thickness(10, 10, 10, 0)
                };

                // Додаємо обробник події для кнопки
                button.Clicked += (sender, e) =>
                {
                    Navigation.PushModalAsync(new EventView(eventItem));
                };

                // Додаємо кнопку до стекового макету
                joinedEventsStackLayout.Children.Add(button);
            }
        }

        var labelOfEvents = new Label()
        {
            Text = "Your events:",
            FontSize = 20,
            Margin = new Thickness(20, 10, 10, 0),
            TextColor = Color.FromArgb("#000000")
        };

        var yourEventsStackLayout = new StackLayout()
        {
            Margin = new Thickness(5)
        };

        foreach (var eventItem in eventList)
        {
            if (eventItem.Email == user.Email)
            {
                var button = new Button()
                {
                    Text = eventItem.Name,
                    TextColor = Color.FromArgb("#FFFFFF"),
                    BackgroundColor = Color.FromArgb("#FF05A27F"),
                    Margin = new Thickness(10, 10, 10, 0)
                };
                // Додаємо обробник події для кнопки
                button.Clicked += (sender, e) =>
                {
                    Navigation.PushModalAsync(new EventManager(eventItem, user));
                };

                // Додаємо кнопку до стекового макету
                yourEventsStackLayout.Children.Add(button);
            }
        }

        var scrollView = new ScrollView()
        {
            Content = new StackLayout()
            {
                Children =
                {
                    image,
                    label,
                    settingsButton,
                    labelOfJoinedEvents,
                    joinedEventsStackLayout,
                    labelOfEvents,
                    yourEventsStackLayout
                }
            },
            BackgroundColor = Color.FromArgb("#E0FFFF")
        };
        var refreshView = new RefreshView
        {
            Content = scrollView,
            Command = new Command(RefreshData),
            RefreshColor = Color.FromArgb("#FF05A27F")
        };
        Content = refreshView;
    }
    private void RefreshData()
    {
        // Вивантажуємо нинішнього користувача та івенти
        string userJson = Preferences.Get("CurrentUser", null);
        user = CustomSQL.GetUserProfile(CustomSQL.Connection(), user.Email, user.Password);
        eventList = CustomSQL.GetEvents(CustomSQL.Connection());

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
            TextColor = Color.FromArgb("#000000"),
            Margin = new Thickness(20, 10, 10, 0)
        };

        var settingsButton = new Button()
        {
            Text = "Settings",
            TextColor = Color.FromArgb("#FFFFFF"),
            BackgroundColor = Color.FromArgb("#FF05A27F"),
            Margin = new Thickness(15, 10, 15, 0)
        };

        // Додаємо обробник події для кнопки налаштувань
        settingsButton.Clicked += (sender, e) =>
        {
            Navigation.PushModalAsync(new Settings());
        };

        var labelOfJoinedEvents = new Label()
        {
            Text = "Joined events:",
            FontSize = 20,
            Margin = new Thickness(20, 20, 10, 0),
            TextColor = Color.FromArgb("#000000")
        };

        var joinedEventsStackLayout = new StackLayout()
        {
            Margin = new Thickness(5)
        };

        foreach (var eventItem in eventList)
        {
            List<string> participants = eventItem.Participants.Split(',').ToList();
            if (participants.Contains(user.Email) && eventItem.Email != user.Email)
            {
                var button = new Button()
                {
                    Text = eventItem.Name,
                    TextColor = Color.FromArgb("#FFFFFF"),
                    BackgroundColor = Color.FromArgb("#FF05A27F"),
                    Margin = new Thickness(10, 10, 10, 0)
                };

                // Додаємо обробник події для кнопки
                button.Clicked += (sender, e) =>
                {
                    Navigation.PushModalAsync(new EventView(eventItem));
                };

                // Додаємо кнопку до стекового макету
                joinedEventsStackLayout.Children.Add(button);
            }
        }

        var labelOfEvents = new Label()
        {
            Text = "Your events:",
            FontSize = 20,
            Margin = new Thickness(20, 10, 10, 0),
            TextColor = Color.FromArgb("#000000")
        };

        var yourEventsStackLayout = new StackLayout()
        {
            Margin = new Thickness(5)
        };

        foreach (var eventItem in eventList)
        {
            if (eventItem.Email == user.Email)
            {
                var button = new Button()
                {
                    Text = eventItem.Name,
                    TextColor = Color.FromArgb("#FFFFFF"),
                    BackgroundColor = Color.FromArgb("#FF05A27F"),
                    Margin = new Thickness(10, 10, 10, 0)
                };
                // Додаємо обробник події для кнопки
                button.Clicked += (sender, e) =>
                {
                    Navigation.PushModalAsync(new EventManager(eventItem, user));
                };

                // Додаємо кнопку до стекового макету
                yourEventsStackLayout.Children.Add(button);
            }
        }

        var scrollView = new ScrollView()
        {
            Content = new StackLayout()
            {
                Children =
                {
                    image,
                    label,
                    settingsButton,
                    labelOfJoinedEvents,
                    joinedEventsStackLayout,
                    labelOfEvents,
                    yourEventsStackLayout
                }
            },
            BackgroundColor = Color.FromArgb("#E0FFFF")
        };
        var refreshView = new RefreshView
        {
            Content = scrollView,
            Command = new Command(RefreshData),
            RefreshColor = Color.FromArgb("#FF05A27F")
        };
        Content = refreshView;
    }
}