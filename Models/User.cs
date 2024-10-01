namespace Ticket_Match.Models;

public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public ICollection<UserTickets> UserTickets { get; set; }
}
public enum Role
{
    Responsible,
    User
}
