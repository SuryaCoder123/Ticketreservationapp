namespace TicketReservationApp.Services
{
    using System.Collections.Generic;
    using Ticketreservationapp.Models;

    public interface IEventService
    {
        Event AddEvent(Event newEvent);
        IEnumerable<Event> GetEvents();
        Booking BookTickets(BookingRequest request);
        bool CancelBooking(TicketCancel tc);
        Booking GetBookingDetails(string bookingReference);
    }
}
