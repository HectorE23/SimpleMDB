namespace SimpleMDB;

public class Movie 
{
    private int v;
    private string fullName;
    private float rating;

    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Description { get; set; }
    public float Rating { get; set; }

    public Movie(int id = 0, string title = "", int year = 2025, string description = "", float rating = 0)
    {
        Id = id;
        Title = title;
        Year = year;
        Description = description;
        Rating = rating;

    }

    public Movie(int v, string fullName, string description, float rating)
    {
        this.v = v;
        this.fullName = fullName;
        Description = description;
        this.rating = rating;
    }

    public override string ToString()
    {
        return $"Movie[Id={Id}, Title={Title}, Year{Year}, Description={Description}, Rating={Rating}]";
    }
}
