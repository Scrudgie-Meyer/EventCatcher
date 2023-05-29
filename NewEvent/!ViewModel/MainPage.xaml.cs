using MySql.Data.MySqlClient;
using NewEvent.Support;
using Newtonsoft.Json;

namespace NewEvent
{
    public partial class MainPage : ContentPage
    {
        private User user =new User();
        private List<Event> eventList = new List<Event>();
        public MainPage()
        {
            InitializeComponent();

            // Вивантажуємо нинішнього користувача та івенти
            string userJson = Preferences.Get("CurrentUser", null);
            user = JsonConvert.DeserializeObject<User>(userJson);
            eventList = CustomSQL.GetEvents(CustomSQL.Connection());

            var label = new Label()
            {
                Text = "Public events",
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                TextColor = Color.FromArgb("#000000"),
                Margin = new Thickness(20, 20, 0, 0)
            };

            var eventsStackLayout = new StackLayout();
            foreach (var eventItem in eventList)
            {
                if (!eventItem.IsPrivate)
                {
                    var button = new Button()
                    {
                        Text = eventItem.Name,
                        TextColor = Color.FromArgb("#FFFFFF"),
                        BackgroundColor = Color.FromArgb("#FF05A27F"),
                        Margin = new Thickness(20, 5, 20, 0)
                    };

                    // Додаємо обробник події для кнопки
                    button.Clicked += (sender, e) =>
                    {
                        Navigation.PushModalAsync(new EventView(eventItem));
                    };

                    // Додаємо кнопку до стекового макету
                    eventsStackLayout.Children.Add(button);
                }
            }

            var scrollView = new ScrollView()
            {
                Content = new StackLayout()
                {
                    Children =
                    {
                        label,
                        eventsStackLayout
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
            user = JsonConvert.DeserializeObject<User>(userJson);
            eventList = CustomSQL.GetEvents(CustomSQL.Connection());

            var label = new Label()
            {
                Text = "Public events",
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                TextColor = Color.FromArgb("#000000"),
                Margin = new Thickness(20, 20, 0, 0)
            };

            var eventsStackLayout = new StackLayout();
            foreach (var eventItem in eventList)
            {
                if (!eventItem.IsPrivate)
                {
                    var button = new Button()
                    {
                        Text = eventItem.Name,
                        TextColor = Color.FromArgb("#FFFFFF"),
                        BackgroundColor = Color.FromArgb("#FF05A27F"),
                        Margin = new Thickness(20, 5, 20, 0)
                    };

                    // Додаємо обробник події для кнопки
                    button.Clicked += (sender, e) =>
                    {
                        Navigation.PushModalAsync(new EventView(eventItem));
                    };

                    // Додаємо кнопку до стекового макету
                    eventsStackLayout.Children.Add(button);
                }
            }

            var scrollView = new ScrollView()
            {
                Content = new StackLayout()
                {
                    Children =
                    {
                        label,
                        eventsStackLayout
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
}