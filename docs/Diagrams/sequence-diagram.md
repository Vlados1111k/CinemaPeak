sequenceDiagram
    actor User
    participant App as BookingService
    participant Repo as ITicketRepository
    participant Store as JsonDataStore
    participant File as tickets.json

    User->>App: BookTicket(row, seat, isVip, strategy)
    App->>Repo: GetAll()
    Repo-->>App: List of tickets
    Note over App: Перевірка LINQ: .Any(t => t.Row == row && t.Seat == seat)
    
    alt Місце зайняте
        App-->>User: Throw InvalidOperationException
    else Місце вільне
        App->>App: CalculateFinalPrice(strategy)
        App->>Repo: Add(new Ticket)
        Repo->>Store: SaveAsync(allTickets)
        Store->>File: Write JSON to disk
        Repo-->>App: Success
        App-->>User: "Квиток заброньовано!"
    end