using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ticket_Match.Data;
using Ticket_Match.Models;
using Ticket_Match.Request;

namespace Ticket_Match.Controllers
{
    public class TicketsController : Controller
    {
        AppDbContext _db = new AppDbContext();
        public IActionResult Index()
        {
            var tickets = new List<Ticket>();
                tickets = _db.Tickets.Include(x => x.UserTickets).ToList();
            return View(tickets);
        }
        [Authorize(Roles = $"{nameof(Role.Responsible)}")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles=$"{nameof(Role.Responsible)}")]
        [HttpPost]
        public IActionResult Create(CreateTicketRequest request)
        {
            var ticket = new Ticket()
            {
                Title=request.Title,
                Description=request.Description,
                TimeOfMatch=request.TimeOfMatch,
                Price=request.Price,
                State=State.Available,
            };
            _db.Add(ticket);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = $"{nameof(Role.Responsible)}")]
        public IActionResult Delete(int id)
        {
            var ticket = _db.Tickets.FirstOrDefault(x => x.TicketId == id)!;
            _db.Tickets.Remove(ticket);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = $"{nameof(Role.Responsible)}")]
        public IActionResult Unavailable(int id)
        {
            var ticket = _db.Tickets.FirstOrDefault(x => x.TicketId == id)!;
            ticket.State = State.Unavailable;
            _db.Update(ticket);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles =$"{nameof(Role.User)}")]
        public IActionResult BookTicket(int TicketId)
        {
            var userId = User.Claims.FirstOrDefault(x=>x.Type==ClaimTypes.Sid)!.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            bool alreadyBooked = _db.UserTickets
                .Any(x => x.UserId == int.Parse(userId) && x.TicketId == TicketId);

            if (alreadyBooked)
            {
                TempData["ErrorMessage"] = "You have already booked this ticket.";
                return RedirectToAction("Index");
            }
            var userticket = new UserTickets()
            {
                UserId = int.Parse(userId),
                TicketId = TicketId
            };
            _db.UserTickets.Add(userticket);
            _db.SaveChanges();
            return RedirectToAction("MyTickets");
        }
        [Authorize(Roles = $"{nameof(Role.User)}")]
        public IActionResult MyTickets()
        {
            var tickets = new List<Ticket>();
            if (User.IsInRole(nameof(User)))
            {
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)!.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    int parsedUserId = int.Parse(userId);
                    tickets = _db.UserTickets.Where(ut => ut.UserId == parsedUserId).Include(ut => ut.Ticket).Select(ut => ut.Ticket).ToList();
                }
            }
            return View(tickets);
        }
        [Authorize(Roles = $"{nameof(Role.User)}")]
        public IActionResult DeleteMyTicket(int id)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;

            if (userId != null)
            {
                var userTicket = _db.UserTickets.FirstOrDefault(x => x.UserId == int.Parse(userId) && x.TicketId == id);

                if (userTicket != null)
                {
                    _db.UserTickets.Remove(userTicket);
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("MyTickets");
        }
    }
}
