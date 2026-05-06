namespace CinemaPeak.Domain.Models;

public class Movie {
    public string Title { get; private set; }
    public int Duration { get; private set; }

    public Movie(string title, int duration) {
        if (string.IsNullOrWhiteSpace(title)) 
            throw new ArgumentException("Назва не може бути порожньою");

        if (duration < 10) 
            throw new ArgumentException("Фільм занадто короткий!");

        Title = title;
        Duration = duration;
    }
}