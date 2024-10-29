namespace TicketReservationApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ticketreservationapp.Data;
    using Ticketreservationapp.Models;
   

    public class EventService : IEventService
    {
        private readonly TicketReservationContext _context;

        public EventService(TicketReservationContext context)
        {
            _context = context;
        }

        public Event AddEvent(Event newEvent)
        {
            if (newEvent == null || string.IsNullOrEmpty(newEvent.Name) || newEvent.AvailableSeats <= 0)
            {
                throw new ArgumentException("Invalid event details.");
            }

            _context.Events.Add(newEvent);
            _context.SaveChanges();
            return newEvent;
        }

        public IEnumerable<Event> GetEvents()
        {
            return _context.Events.ToList();
        }

        public Booking BookTickets(BookingRequest request)
        {
            var evnt = _context.Events.FirstOrDefault(e => e.Name == request.EventName);

            if (evnt == null)
            {
                throw new KeyNotFoundException("Event not found.");
            }

            if (evnt.AvailableSeats < request.Tickets)
            {
                throw new InvalidOperationException("Not enough seats available.");
            }

            evnt.AvailableSeats -= request.Tickets;

            var booking = new Booking
            {
                EventId = evnt.Id,
                Name = evnt.Name,
                UserName = request.UserName,
                Tickets = request.Tickets,
                BookingReference = Guid.NewGuid().ToString()
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return booking;
        }

        public bool CancelBooking(TicketCancel tc)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingReference == tc.Referrence);
            if (booking == null)
            {
                throw new KeyNotFoundException("Booking not found.");
            }

            var evnt = _context.Events.Find(booking.EventId);
            if (evnt != null)
            {
                evnt.AvailableSeats += booking.Tickets;
            }

            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            return true;
        }

        public Booking GetBookingDetails(string bookingReference)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingReference == bookingReference);
            if (booking == null)
            {
                throw new KeyNotFoundException("Booking not found.");
            }

            return booking;
        }
    }
}
