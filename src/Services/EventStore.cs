using EventSourcingDemo.Models;

namespace EventSourcingDemo.Services
{
    public class EventStore
    {
        private readonly List<Event> _events;

        public EventStore()
        {
            _events = new List<Event>();
        }

        public void AddEvent(Event eventItem)
        {
            Console.WriteLine($"Adding event: {eventItem.Name} for StreamId: {eventItem.StreamId}");
            _events.Add(eventItem);
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return _events;
        }
    }
}