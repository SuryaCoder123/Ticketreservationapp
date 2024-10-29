using Microsoft.AspNetCore.Mvc;
using Ticketreservationapp.Models;
using TicketReservationApp.Services;

namespace TicketReservationApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("Post")]
        public ActionResult<Event> AddEvent([FromBody] Event newEvent)
        {
            try
            {
                var createdEvent = _eventService.AddEvent(newEvent);
                return Ok(createdEvent);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Event>> GetEvents()
        {
            return Ok(_eventService.GetEvents());
        }

        [HttpPost("book")]
        public ActionResult<Booking> BookTickets([FromBody] BookingRequest request)
        {
            try
            {
                var booking = _eventService.BookTickets(request);
                return Ok(booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("cancel")]
        public ActionResult CancelBooking([FromBody] TicketCancel tc)
        {
            try
            {
                _eventService.CancelBooking(tc);
                return Ok(new { message = "Booking canceled successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("booking")]
        public ActionResult<Booking> GetBookingDetails(string bookingReference)
        {
            try
            {
                var booking = _eventService.GetBookingDetails(bookingReference);
                return Ok(booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
