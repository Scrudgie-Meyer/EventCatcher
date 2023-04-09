using MySql.Data.MySqlClient;
using NewEvent.Support;

namespace NewEvent;

public partial class EventCreator : ContentPage
{
    private bool IsPrivate = false; 
	public EventCreator()
	{
		InitializeComponent();
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
    private void OnCreateClicked(object sender, EventArgs e)
    {
        //З'єднання з базою даних
        string Conn = "Server=sql7.freemysqlhosting.net;Port=3306;Database=sql7611982;Uid=sql7611982;Pwd=Aegl446cFD;";
        MySqlConnection connection = new MySqlConnection(Conn);
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }

        //Збереження даних
        MySqlCommand command = new MySqlCommand($"INSERT INTO Events (Name, Date, Location, Description, IsPrivate) VALUES (@Name, @Date, @Location, @Description, @IsPrivate)", connection);
        command.Parameters.AddWithValue("@Name", Name.Text);
        command.Parameters.AddWithValue("@Date", Date.Date);
        //command.Parameters.AddWithValue("@Location", );
        command.Parameters.AddWithValue("@Description", Description.Text);
        command.Parameters.AddWithValue("@IsPrivate", IsPrivate);
        command.ExecuteNonQuery();
    }
}