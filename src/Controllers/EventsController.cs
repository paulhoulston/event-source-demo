using Microsoft.AspNetCore.Mvc;
using EventSourcingDemo.Models;
using EventSourcingDemo.Services;
using System.Collections.Generic;
using System;

namespace EventSourcingDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventStore _eventStore;
        private readonly SubscriberService _subscriberService;

        public EventsController(EventStore eventStore, SubscriberService subscriberService)
        {
            _eventStore = eventStore;
            _subscriberService = subscriberService;
        }

        [HttpPost("publish")]
        public ActionResult<Event> PublishEvent([FromBody] Event newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest("Event cannot be null.");
            }

            newEvent.Timestamp = DateTime.UtcNow;
            _eventStore.AddEvent(newEvent);
            _subscriberService.NotifySubscribers(newEvent);

            return CreatedAtAction(nameof(PublishEvent), new { id = newEvent.Id }, newEvent);
        }

        [HttpPost("subscribe")]
        public ActionResult Subscribe([FromBody] string subscriberUrl)
        {
            if (string.IsNullOrWhiteSpace(subscriberUrl))
            {
                return BadRequest("Subscriber URL cannot be empty.");
            }

            _subscriberService.AddSubscriber(subscriberUrl);
            return Ok("Subscriber added successfully.");
        }

        [HttpGet]
        public ActionResult<List<Event>> GetAllEvents()
        {
            var events = _eventStore.GetAllEvents();
            return Ok(events);
        }
    }
}