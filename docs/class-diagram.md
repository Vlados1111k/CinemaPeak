# Діаграма класів CinemaPeak

```mermaid
classDiagram
    class Movie {
        +String Title
        +Int Duration
        +Movie(title, duration)
    }
    class Ticket {
        <<abstract>>
        +Int Row
        +Int Seat
        +Decimal BasePrice
        +CalculateFinalPrice()*
    }
    class StandardTicket {
        +CalculateFinalPrice()
    }
    class VipTicket {
        +CalculateFinalPrice()
    }
    class ITicketRepository {
        <<interface>>
        +Add(Ticket)
        +GetAll()
    }

    Ticket <|-- StandardTicket : успадковує
    Ticket <|-- VipTicket : успадковує
    ITicketRepository ..> Ticket : використовує