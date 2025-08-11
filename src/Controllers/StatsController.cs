using Microsoft.AspNetCore.Mvc;
using EventSourcingDemo.Models;
using EventSourcingDemo.Services;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EventSourcingDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly EventStore _eventStore;

        public StatsController(EventStore eventStore)
        {
            _eventStore = eventStore;
        }

        /// <summary>
        /// Endpoint to receive events as a subscriber.
        /// </summary>
        [HttpPost("receive")]
        public IActionResult ReceiveEvent([FromBody] Event mortgageEvent)
        {
            if (mortgageEvent == null || string.IsNullOrWhiteSpace(mortgageEvent.StreamId))
                return BadRequest("Invalid event data.");

            // For demonstration, just log the event receipt
            Console.WriteLine($"StatsController received event: {mortgageEvent.Name} for StreamId: {mortgageEvent.StreamId}");

            // Optionally, you could update some in-memory stats here

            return Ok();
        }

        /// <summary>
        /// Returns the number of mortgage streams that are not yet completed (no FundsReleased event).
        /// </summary>
        [HttpGet("not-completed")]
        public ActionResult<int> GetNotCompletedCount()
        {
            var events = _eventStore.GetAllEvents();

            // Group events by StreamId and get the latest event for each stream
            var latestEvents = events
                .GroupBy(e => e.StreamId)
                .Select(g => g.OrderByDescending(e => e.Timestamp).FirstOrDefault());

            // Count streams where the latest event is NOT FundsReleased
            int notCompletedCount = latestEvents.Count(e => e != null && e.Name != "FundsReleased");

            return Ok(notCompletedCount);
        }
    }
}