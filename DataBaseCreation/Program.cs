using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Program
{
    public static void Main()
    {
        Creation();
    }
    public static void Creation()
    {
        using var dbContext = new LibraryContext();
        dbContext.Database.EnsureCreated();
    }
}
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Nickname { get; set; }
    public string Password { get; set; }

}
public class LibraryContext : DbContext
{
    public DbSet<User> Users { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Volodymyr_Petrash\Practice\EventCatcher\NewEvent\EventData.mdf;Integrated Security=True");
    }
}

[Table("Users1")]
public class UsersTable
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Column("Email")]
    public string Email { get; set; }
    [Required]
    [Column("Nickname")]
    public string Nickname { get; set; }
    [Required]
    [Column("Password")]
    public string Password { get; set; }

}