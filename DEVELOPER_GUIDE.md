# Посібник розробника (Developer Guide) — CinemaPeak

## 1. Архітектурний дизайн
Проєкт побудовано за принципами **Clean Architecture**:
* `CinemaPeak.Domain`: Чисті сутності (`Ticket`, `StandardTicket`, `VipTicket`) та інтерфейси. Нуль зовнішніх залежностей.
* `CinemaPeak.Application`: Бізнес-сценарії (`BookingService`, `AnalyticsService`).
* `CinemaPeak.Infrastructure`: Доступ до дисків (`JsonDataStore`, `TicketRepository`).

## 2. Правила розширення системи (SOLID)
* **Додавання нової знижки:** Створіть новий клас у папці `Strategies`, що реалізує інтерфейс `IDiscountStrategy` (Дотримання Open/Closed Principle).
* **Зміна сховища даних:** Реалізуйте інтерфейс `ITicketRepository` для нової БД (наприклад, PostgreSQL), не змінюючи код сервісів (Dependency Inversion Principle).

## 3. Запуск тестів
```bash
dotnet test /p:CollectCoverage=true