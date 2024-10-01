namespace Ticket_Match.Models
{
    public class UserTickets
    {
        public int UserTicketId { get; set; }  
        public int UserId { get; set; }
        public User User { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }
}
