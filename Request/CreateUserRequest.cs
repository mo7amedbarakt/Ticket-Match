using System.ComponentModel.DataAnnotations;

namespace Ticket_Match.Request;

public class CreateUserRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(40)]
    public string Name { get; set; }
    [Required]
    [MinLength(10)]
    [MaxLength(50)]
    public string Email { get; set; }
    [Required]
    [MinLength(11)]
    [MaxLength(11)]
    public string Phone { get; set; }
    [Required]
    [MinLength(6)]
    [MaxLength(20)]
    public string Password { get; set; }

}
