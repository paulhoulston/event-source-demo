# Event Sourcing Demo

This project demonstrates the concept of event sourcing in a .NET Core application. It allows users to publish events and manage subscribers effectively.

## Project Structure

The project is organized as follows:

```
event-sourcing-demo
├── src
│   ├── EventSourcingDemo
│   │   ├── Models
│   │   │   └── Event.cs          # Defines the Event class with properties Id, Name, and Timestamp.
│   │   ├── Controllers
│   │   │   └── EventsController.cs # Exports the EventsController class for publishing events and managing subscribers.
│   │   ├── Services
│   │   │   ├── EventStore.cs      # Manages the storage of events.
│   │   │   └── SubscriberService.cs # Manages event subscribers.
│   │   ├── Program.cs             # Entry point of the application.
│   │   └── Startup.cs             # Configures services and the application's request pipeline.
├── EventSourcingDemo.sln           # Solution file for the .NET Core application.
└── README.md                       # Documentation for the project.
```

## Getting Started

To set up and run the application, follow these steps:

1. **Clone the repository**:
   ```
   git clone <repository-url>
   cd event-sourcing-demo
   ```

2. **Navigate to the project directory**:
   ```
   cd src/EventSourcingDemo
   ```

3. **Restore dependencies**:
   ```
   dotnet restore
   ```

4. **Run the application**:
   ```
   dotnet run --project EventSourcingDemo.csproj
   ```

## Usage

### Publishing Events

To publish a new event, use the following `curl` command:

```sh
curl -X POST "http://localhost:5000/api/events/publish" \
     -H "Content-Type: application/json" \
     -d '{"name": "MyEvent"}'
```

### Managing Subscribers

To add a new subscriber, use:

```sh
curl -X POST "http://localhost:5000/api/events/subscribe" \
     -H "Content-Type: application/json" \
     -d '"http://subscriber-url/callback"'
```

### Retrieving All Events

To get all published events:

```sh
curl "http://localhost:5000/api/events"
```

> **Note:**  
> If your app is running on a different port or using HTTPS, adjust the URLs accordingly.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for any suggestions or improvements.

## License

This project is licensed under the MIT License. See the LICENSE file for details.