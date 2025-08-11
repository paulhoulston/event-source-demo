# Event Source Demo

This project demonstrates event sourcing concepts using a simple mortgage lifecycle example.

## Getting Started

### Prerequisites

- [.NET 7 SDK or later](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)
- (Optional) [Postman](https://www.postman.com/) or `curl` for testing API endpoints

### Setup

1. **Clone the repository:**
   ```sh
   git clone https://github.com/your-username/event-source-demo.git
   cd event-source-demo
   ```

2. **Restore dependencies:**
   ```sh
   dotnet restore
   ```

3. **Build the project:**
   ```sh
   dotnet build
   ```

4. **Run the application:**
   ```sh
   dotnet run --project src
   ```
   The API will start (by default on `http://localhost:5000` or `http://localhost:5000`).

---

## Mortgage Event Lifecycle

A mortgage in this demo follows a simplified lifecycle, represented by three main events:

1. **ApplicationReceived** (`apply`)  
   The mortgage application is received.

2. **ApplicationDecided** (`decide`)  
   A decision is made on the mortgage application.

3. **FundsReleased** (`complete`)  
   The mortgage is completed and funds are released.

Each event is associated with a `StreamId`, which represents a unique mortgage entity. All events for a mortgage are grouped by this `StreamId`.

---

## API Usage (with `curl`)

### 1. Apply for a Mortgage

```sh
curl -X POST http://localhost:5000/api/events/apply \
  -H "Content-Type: application/json" \
  -d "\"your-mortgage-id\""
```

Creates an `ApplicationReceived` event for the given mortgage.

---

### 2. Decide on a Mortgage Application

```sh
curl -X POST http://localhost:5000/api/events/decide \
  -H "Content-Type: application/json" \
  -d "\"your-mortgage-id\""
```

Creates an `ApplicationDecided` event for the given mortgage.

---

### 3. Complete the Mortgage

```sh
curl -X POST http://localhost:5000/api/events/complete \
  -H "Content-Type: application/json" \
  -d "\"your-mortgage-id\""
```

Creates a `FundsReleased` event for the given mortgage.

---

### 4. Get All Events

```sh
curl http://localhost:5000/api/events
```

Returns a list of all events.

---

### 5. Subscribe to Events

You can subscribe an external service to receive notifications when new events are published.

```sh
curl -X POST http://localhost:5000/api/events/subscribe \
  -H "Content-Type: application/json" \
  -d "\"https://your-subscriber-endpoint.com/api/receive\""
```

To subscribe the built-in current state endpoint:

```sh
curl -X POST http://localhost:5000/api/events/subscribe \
  -H "Content-Type: application/json" \
  -d "\"http://localhost:5000/api/currentstate/receive\""
```

---

## Replaying Events

The **Replay Events** feature allows you to resend all previously stored events to all currently registered subscribers. This is useful for rebuilding the state of downstream systems or new subscribers that need to catch up with the event history.

### How to Replay Events

You can trigger a replay of all events using the following `curl` command:

```sh
curl -X POST http://localhost:5000/api/events/replay
```

This will send every event in the event store to all registered subscriber endpoints in the order they were originally received.

---

## Event Structure

Each event has the following structure:

```json
{
  "id": 1,
  "streamId": "your-mortgage-id",
  "name": "ApplicationReceived | ApplicationDecided | FundsReleased",
  "timestamp": "2025-08-09T12:00:00Z"
}
```

---

## Summary

This demo shows how event sourcing can be used to track the lifecycle of a mortgage by recording each significant event in the process. Events are grouped by `StreamId` to represent the history of each mortgage.

### Full demo CURL commands

```sh
{
   curl -X POST http://localhost:5000/api/events/subscribe \
   -H "Content-Type: application/json" \
   -d "\"http://localhost:5000/api/currentstate/receive\""

   curl -X POST http://localhost:5000/api/events/subscribe \
   -H "Content-Type: application/json" \
   -d "\"http://localhost:5000/api/stats/receive\""

   curl -X POST http://localhost:5000/api/events/apply \
   -H "Content-Type: application/json" \
   -d "\"paul-1\""

   curl -X POST http://localhost:5000/api/events/apply \
   -H "Content-Type: application/json" \
   -d "\"jamie\""
   

   curl -X POST http://localhost:5000/api/events/decide \
   -H "Content-Type: application/json" \
   -d "\"paul-1\""

   curl -X POST http://localhost:5000/api/events/complete \
   -H "Content-Type: application/json" \
   -d "\"paul-1\""

   curl http://localhost:5000/api/events
   curl http://localhost:5000/api/currentstate
   curl http://localhost:5000/api/stats/not-completed

}
```