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
            _events.Add(eventItem);
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return _events;
        }
    }
}