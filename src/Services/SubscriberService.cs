using EventSourcingDemo.Models;
using System.Collections.Generic;

namespace EventSourcingDemo.Services
{
    public class SubscriberService
    {
        private readonly List<string> _subscribers = new();

        public void AddSubscriber(string url)
        {
            if (!_subscribers.Contains(url))
                _subscribers.Add(url);
        }

        public void NotifySubscribers(Event eventToNotify)
        {
            // For demo: just a stub. In a real app, you'd POST to each subscriber URL.
        }
    }
}