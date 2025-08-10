using Microsoft.AspNetCore.Mvc;
using EventSourcingDemo.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System;

namespace EventSourcingDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrentStateController : ControllerBase
    {
        // In-memory store for current state of each mortgage (StreamId)
        private static readonly ConcurrentDictionary<string, string> MortgageStates = new();

        /// <summary>
        /// Subscriber endpoint to receive events and update mortgage state.
        /// </summary>
        [HttpPost("receive")]
        public IActionResult ReceiveEvent([FromBody] Event mortgageEvent)
        {
            if (mortgageEvent == null || string.IsNullOrWhiteSpace(mortgageEvent.StreamId))
                return BadRequest("Invalid event data.");

            // Update the state based on event name
            string state = mortgageEvent.Name switch
            {
                "ApplicationReceived" => "Application Received",
                "ApplicationDecided" => "Application Decided",
                "FundsReleased" => "Funds Released",
                _ => "Unknown"
            };

            MortgageStates[mortgageEvent.StreamId] = state;

            // Output current state to console
            Console.WriteLine($"Mortgage '{mortgageEvent.StreamId}' is now at state: {state}");

            return Ok();
        }

        /// <summary>
        /// Get the current state of all mortgages.
        /// </summary>
        [HttpGet]
        public ActionResult<Dictionary<string, string>> GetAllCurrentStates()
        {
            return Ok(MortgageStates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        /// <summary>
        /// Get the current state of a specific mortgage.
        /// </summary>
        [HttpGet("{streamId}")]
        public ActionResult<string> GetCurrentState(string streamId)
        {
            if (MortgageStates.TryGetValue(streamId, out var state))
                return Ok(state);

            return NotFound("Mortgage not found.");
        }
    }
}