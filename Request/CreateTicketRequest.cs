using System.ComponentModel.DataAnnotations;

namespace Ticket_Match.Request
{
    public class CreateTicketRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime TimeOfMatch { get; set; }
        [Required]
        public string Price { get; set; }
    }
}
