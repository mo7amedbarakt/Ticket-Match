using Microsoft.EntityFrameworkCore;
using Ticket_Match.Models;

namespace Ticket_Match.Data;

public class AppDbContext:DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<UserTickets> UserTickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database = Tickets;Integrated Security=true;Encrypt=false");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserTickets>()
            .HasKey(x => x.UserTicketId);

        modelBuilder.Entity<UserTickets>()
            .HasOne(x => x.User)
            .WithMany(x => x.UserTickets)
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<UserTickets>()
            .HasOne(x => x.Ticket)
            .WithMany(x => x.UserTickets)
            .HasForeignKey(x => x.TicketId);

        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasData(new User
        {
            UserId = 1,
            Name = "Mohamed Ali",
            Email = "Mohamed1@gmail.com",
            Phone = "01015648974",
            Password = "654321",
            Role = Role.Responsible
        });
    }
}
