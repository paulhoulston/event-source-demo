using System;

namespace EventSourcingDemo.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string StreamId { get; set; } = string.Empty; // Added to associate event with a stream/entity
        public string Name { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}