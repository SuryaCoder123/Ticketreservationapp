using NUnit.Framework;
using TicketReservationApp.Services;
using Ticketreservationapp.Data;
using Ticketreservationapp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace TicketReservationApp.Tests
{
    [TestFixture]
    public class EventServiceTests
    {
        private TicketReservationContext _context;
        private EventService _eventService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TicketReservationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TicketReservationContext(options);
            _eventService = new EventService(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void AddEvent_ValidEvent_ShouldAddEvent()
        {
            var newEvent = new Event
            {
                Name = "Concert",
                Date = DateTime.Now,
                Venue = "City Hall",
                TotalSeats = 150,
                AvailableSeats = 100
            };
            var result = _eventService.AddEvent(newEvent);

            Assert.IsNotNull(result);
            Assert.AreEqual("Concert", result.Name);
            Assert.AreEqual(100, result.AvailableSeats);
            Assert.AreEqual("City Hall", result.Venue);
            Assert.AreEqual(150, result.TotalSeats);
            Assert.AreEqual(1, _context.Events.Count());
        }

        [Test]
        public void GetEvents_ShouldReturnAllEvents()
        {
            _context.Events.Add(new Event { Name = "Concert", AvailableSeats = 100, TotalSeats = 150, Date = DateTime.Now, Venue = "City Hall" });
            _context.Events.Add(new Event { Name = "Play", AvailableSeats = 50, TotalSeats = 100, Date = DateTime.Now, Venue = "Community Theater" });
            _context.SaveChanges();

            var events = _eventService.GetEvents();

            Assert.AreEqual(2, events.Count());
        }

        [Test]
        public void BookTickets_ValidRequest_ShouldReturnBooking()
        {
            var newEvent = new Event { Name = "Concert", AvailableSeats = 100, TotalSeats = 150, Date = DateTime.Now, Venue = "City Hall" };
            _context.Events.Add(newEvent);
            _context.SaveChanges();

            var request = new BookingRequest { EventName = "Concert", UserName = "JohnDoe", Tickets = 2 };
            var booking = _eventService.BookTickets(request);

            Assert.IsNotNull(booking);
            Assert.AreEqual("Concert", booking.Name);
            Assert.AreEqual("JohnDoe", booking.UserName);
            Assert.AreEqual(2, booking.Tickets);
            Assert.AreEqual(98, newEvent.AvailableSeats);
        }

        [Test]
        public void CancelBooking_ValidBookingReference_ShouldCancelBooking()
        {
            var newEvent = new Event { Name = "Concert", AvailableSeats = 100, TotalSeats = 150, Date = DateTime.Now, Venue = "City Hall" };
            _context.Events.Add(newEvent);
            _context.SaveChanges();

            var booking = new Booking { EventId = newEvent.Id, Name = newEvent.Name, UserName = "JohnDoe", Tickets = 2, BookingReference = Guid.NewGuid().ToString() };
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            var ticketCancel = new TicketCancel { Referrence = booking.BookingReference };
            var result = _eventService.CancelBooking(ticketCancel);

            Assert.IsTrue(result);
            Assert.AreEqual(102, newEvent.AvailableSeats);
            Assert.AreEqual(0, _context.Bookings.Count());
        }

        [Test]
        public void GetBookingDetails_ValidBookingReference_ShouldReturnBooking()
        {
            var bookingReference = Guid.NewGuid().ToString();
            var booking = new Booking { Name = "Concert", UserName = "JohnDoe", Tickets = 2, BookingReference = bookingReference };
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            var result = _eventService.GetBookingDetails(bookingReference);

            Assert.IsNotNull(result);
            Assert.AreEqual("Concert", result.Name);
            Assert.AreEqual("JohnDoe", result.UserName);
            Assert.AreEqual(2, result.Tickets);
        }
    }
}
