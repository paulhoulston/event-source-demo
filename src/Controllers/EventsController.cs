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

        // Endpoint for ApplicationReceived event
        [HttpPost("apply")]
        public ActionResult<Event> ApplyForMortgage([FromBody] string streamId)
        {
            if (string.IsNullOrWhiteSpace(streamId))
            {
                return BadRequest("StreamId (Mortgage Id) is required.");
            }

            var newEvent = new Event
            {
                StreamId = streamId,
                Name = "ApplicationReceived",
                Timestamp = DateTime.UtcNow
            };

            _eventStore.AddEvent(newEvent);
            _subscriberService.NotifySubscribers(newEvent);

            return CreatedAtAction(nameof(ApplyForMortgage), new { id = newEvent.Id }, newEvent);
        }

        // Endpoint for ApplicationDecided event
        [HttpPost("decide")]
        public ActionResult<Event> DecideApplication([FromBody] string streamId)
        {
            if (string.IsNullOrWhiteSpace(streamId))
            {
                return BadRequest("StreamId (Mortgage Id) is required.");
            }

            var newEvent = new Event
            {
                StreamId = streamId,
                Name = "ApplicationDecided",
                Timestamp = DateTime.UtcNow
            };

            _eventStore.AddEvent(newEvent);
            _subscriberService.NotifySubscribers(newEvent);

            return CreatedAtAction(nameof(DecideApplication), new { id = newEvent.Id }, newEvent);
        }

        // Endpoint for FundsReleased event
        [HttpPost("complete")]
        public ActionResult<Event> CompleteMortgage([FromBody] string streamId)
        {
            if (string.IsNullOrWhiteSpace(streamId))
            {
                return BadRequest("StreamId (Mortgage Id) is required.");
            }

            var newEvent = new Event
            {
                StreamId = streamId,
                Name = "FundsReleased",
                Timestamp = DateTime.UtcNow
            };

            _eventStore.AddEvent(newEvent);
            _subscriberService.NotifySubscribers(newEvent);

            return CreatedAtAction(nameof(CompleteMortgage), new { id = newEvent.Id }, newEvent);
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