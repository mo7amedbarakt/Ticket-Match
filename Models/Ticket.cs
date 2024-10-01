namespace Ticket_Match.Models;

public class Ticket
{
    public int TicketId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime TimeOfMatch { get; set; }
    public State State { get; set; }
    public string Price { get; set; }
    public ICollection<UserTickets> UserTickets { get; set; }
}
public enum State
{
    Available,
    Unavailable
}
