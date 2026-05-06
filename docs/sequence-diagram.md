# Діаграма послідовності: Бронювання квитка

```mermaid
sequenceDiagram
    participant User as Користувач (Console)
    participant Service as BookingService (Application)
    participant Repo as TicketRepository (Infrastructure)
    participant Domain as Ticket (Domain)

    User->>Service: CreateBooking(row, seat, isVip)
    Service->>Domain: New Ticket(row, seat)
    Domain-->>Service: Об'єкт створено
    Service->>Repo: Add(ticket)
    Repo-->>Service: Збережено
    Service-->>User: "Успішно заброньовано!"