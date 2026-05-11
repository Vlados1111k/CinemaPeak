# Діаграма послідовності: Процес бронювання

```mermaid
sequenceDiagram
    participant U as Користувач
    participant S as BookingService
    participant R as TicketRepository
    participant D as JsonDataStore

    U->>S: BookTicket(ряд, місце, VIP, знижка)
    S->>R: GetAll()
    R-->>S: Список існуючих квитків
    Note over S: Перевірка LINQ: .Any(...)
    
    alt Місце вже зайняте
        S-->>U: Помилка: InvalidOperationException
    else Місце вільне
        S->>S: Розрахунок ціни (Strategy)
        S->>R: Add(новий квиток)
        R->>D: SaveAsync(оновлений список)
        D-->>R: Збережено у файл
        R-->>S: Підтвердження
        S-->>U: Повідомлення "Заброньовано!"
    end
