# Матриця відповідності Use Cases та Тестів

Ідентифікатор Use Case, опис бізнес-сценарію, назва тестів у коді (CinemaTests.cs), статус.

**UC-1: Бронювання**  Успішне створення квитка, якщо місце вільне  `BookingService_ShouldBookTicket_WhenSeatIsFree`  **Passed** 
**UC-1: Негативний**  Заборона подвійного бронювання на одне місце  `BookingService_ShouldThrowException_IfSeatOccupied`  **Passed** 
**UC-2: Знижки**  Перевірка 20% знижки для студентів  `StudentDiscount_ShouldApply20Percent`  **Passed** 
**UC-3: Збереження** Повний цикл запису на диск та відновлення  `SaveAndReload_ShouldPreserveDataIntegrity`  **Passed** 
**UC-3: Збої I/O**  Захист системи при пошкодженні JSON файлу  `LoadAsync_ShouldHandleCorruptedJsonGracefully_FaultHandling`  **Passed** 