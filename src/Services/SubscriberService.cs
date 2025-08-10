using EventSourcingDemo.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventSourcingDemo.Services
{
    public class SubscriberService
    {
        private readonly List<string> _subscribers = new();
        private static readonly HttpClient _httpClient = new();

        public void AddSubscriber(string url)
        {
            if (!_subscribers.Contains(url))
                _subscribers.Add(url);
        }

        public void NotifySubscribers(Event eventToNotify)
        {
            Console.WriteLine($"Notifying subscribers about event: {eventToNotify.Name} for StreamId: {eventToNotify.StreamId}");
            var json = JsonSerializer.Serialize(eventToNotify);

            foreach (var url in _subscribers)
            {
                Console.WriteLine($"Sending notification to subscriber: {url}");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                try
                {
                    // Synchronous HTTP POST
                    var response = _httpClient.PostAsync(url, content).GetAwaiter().GetResult();
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Failed to notify subscriber {url}: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error notifying subscriber {url}: {ex.Message}");
                }
            }
        }
    }
}