
namespace NewEvent
{
    internal class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public User()
        {
            Id = -1;
            Email = "1";
            Nickname = "2";
            Password = "3";
        }
    }
}
