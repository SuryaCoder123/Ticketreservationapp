using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ticketreservationapp.Models;

namespace Ticketreservationapp.Data
{
    public class TicketReservationContext : DbContext
    {
        public TicketReservationContext(DbContextOptions<TicketReservationContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasData(
                new Event { Id = 1, Name = "Tech Conference 2024", Date = new DateTime(2024, 12, 5), Venue = "New York Hall", TotalSeats = 200, AvailableSeats = 200 },
                new Event { Id = 2, Name = "Comedy Show with John Doe", Date = new DateTime(2024, 11, 10), Venue = "LA Theater", TotalSeats = 150, AvailableSeats = 150 }
            );
        }
    }
}
