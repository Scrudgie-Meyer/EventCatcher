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

        // ����������� ��������� �����������
        string userJson = Preferences.Get("CurrentUser", null);
        user = JsonConvert.DeserializeObject<User>(userJson);

        var image = new Image()
        {
            Source = "profilephoto.jfif",
            Margin = new Thickness(0, 5, 0, 0)
        };

        // ������ ���� � ��'�� �����������
        var label = new Label()
        {
            Text = $"User name: {user.Nickname}",
            FontSize = 20,
            TextColor = Color.FromArgb("#FFFFFF"),
            Margin = new Thickness(0, 20, 0, 0)
        };

        // ������ ������ ��� �������� �����������
        var settingsButton = new Button()
        {
            Text = "Settings",
            TextColor = Color.FromArgb("#FFFFFF"),
            BackgroundColor = Color.FromArgb("#023402"),
            Margin = new Thickness(0, 20, 0, 0)
        };

        // ������ �������� ��䳿 ��� ������ �����������
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

        // ��������� �������� ����� � ������ �� ����� ���� �� ������
        var stackLayout = new StackLayout()
        {
            VerticalOptions = LayoutOptions.StartAndExpand,
            Margin = new Thickness(20)
        };
        stackLayout.Children.Add(image);
        stackLayout.Children.Add(label);
        stackLayout.Children.Add(settingsButton);
        stackLayout.Children.Add(labelOfEvents);

        // ����������� ������ ���� �����������
        string events = Preferences.Get("Events", null);
        List<Event> eventList = JsonConvert.DeserializeObject<List<Event>>(events);

        // ������ ������ ��� ����� ��䳿 �����������
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

                // ������ �������� ��䳿 ��� ������
                button.Clicked += (sender, e) =>
                {
                    Navigation.PushModalAsync(new EventManager(eventItem, user));
                };

                // ������ ������ �� ��������� ������
                stackLayout.Children.Add(button);
            }
        }

        // ������������ �������� ����� �� ���� �������
        Content = stackLayout;
    }

}