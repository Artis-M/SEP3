# 3 Tier Arhitecture Chat System using Blazor

- Tier 1 - Blazor WASM.
- Tier 2 - Application Layer in C#.
- Tier 3 - Database(MongoDB) in Java.


### Tier 1 - Blazor Client

Tier 1 (Client) uses Blazor WASM for the front-end. it communicated using SignalR for instant messaging and for retrieving and storing data that does not require low latency webservices are used.

### Tier 2 - Application (Server)

Tier 2 (Server) is the SignalR hub for the client and also exposes webservices. It processes the data received from the client and receives it from the database layer as needed.

### Tier 3 - Database

Tier 3 (Database) uses MongoDB to store and retrieve info for the chat system.

### Security Concerns

Security was not considered for the system. The only real quick security implementation is password hashing on the Blazor client for data anonymity when saving passwords to the database and when being transferred between the tiers. The web service has no authorization whatsoever but implementing it using the existing authentication for the Blazor client should not be too much of an issue.
