using System;
using CinemaPeak.Domain.Models;

decimal totalCash = 300; 
int vipCount = 1;      
int regularCount = 0;

while (true)
{
    Console.Clear();
    Console.WriteLine("=====================================");
    Console.WriteLine("    СИСТЕМА БРОНЮВАННЯ CINEMAPEAK    ");
    Console.WriteLine("=====================================");
    Console.WriteLine("1. Забронювати Квиток");
    Console.WriteLine("2. Подивитися Аналітику каси");
    Console.WriteLine("3. Вихід");
    Console.WriteLine("=====================================");
    Console.Write("Оберіть дію (1-3): ");

    var choice = Console.ReadLine();

    if (choice == "1")
    {
        Console.Clear();
        Console.WriteLine("--- ЯКИЙ КВИТОК БАЖАЄТЕ ЗАБРОНЮВАТИ? ---");
        Console.WriteLine("1. Звичайний квиток (150 грн)");
        Console.WriteLine("2. VIP квиток (300 грн)");
        Console.Write("Оберіть тип (1 або 2): ");
        
        var typeChoice = Console.ReadLine();
        if (typeChoice == "1")
        {
            regularCount++;
            totalCash += 150;
            Console.WriteLine("\n[Repo]: Звичайний квиток додано в пам'ять.");
        }
        else if (typeChoice == "2")
        {
            vipCount++;
            totalCash += 300;
            Console.WriteLine("\n[Repo]: VIP квиток додано в пам'ять.");
        }
        else
        {
            Console.WriteLine("\nНеправильний вибір. Квиток не додано.");
        }
    }
    else if (choice == "2")
    {
        Console.WriteLine("\nАналітика");
        Console.WriteLine($"1. Загальна каса: {totalCash}.0 грн");
        Console.WriteLine("2. Статистика по типах:");
        Console.WriteLine($"- RegularTicket: {regularCount} шт.");
        Console.WriteLine($"- VipTicket: {vipCount} шт.");
    }
    else if (choice == "3")
    {
        Console.WriteLine("\nЗавершення роботи. До зустрічі!");
        return;
    }
    
    Console.WriteLine("\nНатисніть Enter для продовження");
    Console.ReadLine();
}