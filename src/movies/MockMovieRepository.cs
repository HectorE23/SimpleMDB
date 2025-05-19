namespace SimpleMDB;

public class MockMovieRepository : IMovieRepository
{
    private List<Movie> movies;
    private int idCount;

    public MockMovieRepository()
    {
        movies = new List<Movie>();
        idCount = 0;

        string[] titles = {
            "The Shawshank Redemption", "Inception", "The Dark Knight", "Pulp Fiction",
            "Forrest Gump", "The Matrix", "Fight Club", "Interstellar",
            "Gladiator", "The Lord of the Rings: The Fellowship of the Ring",
            "The Godfather", "The Lion King", "Saving Private Ryan", "Jurassic Park", "Titanic"
        };

        int[] years = {
            1994, 2010, 2008, 1994,
            1994, 1999, 1999, 2014,
            2000, 2001,
            1972, 1994, 1998, 1993, 1997
        };

        string[] descriptions = {
            "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
            "A thief who steals corporate secrets through dream-sharing technology is given the inverse task of planting an idea.",
            "Batman battles the Joker in Gotham City while struggling with his own moral code.",
            "The lives of two mob hitmen, a boxer, and a pair of diner bandits intertwine in four tales of violence and redemption.",
            "The presidencies of Kennedy and Johnson, the Vietnam War, and more, seen through the eyes of Forrest Gump.",
            "A hacker discovers the world he lives in is a simulated reality and joins a rebellion.",
            "An insomniac office worker and a soap maker form an underground fight club.",
            "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
            "A former Roman General sets out to exact vengeance against the corrupt emperor.",
            "A meek Hobbit and eight companions set out on a journey to destroy a powerful ring.",
            "The aging patriarch of an organized crime dynasty transfers control to his reluctant son.",
            "A lion prince flees his kingdom only to learn the true meaning of responsibility and bravery.",
            "During WWII, a group of U.S. soldiers go behind enemy lines to retrieve a paratrooper.",
            "During a preview tour, a theme park suffers a major power breakdown that allows dinosaurs to run amok.",
            "A seventeen-year-old aristocrat falls in love with a kind but poor artist aboard the luxurious Titanic."
        };

        Random rand = new Random();

        for (int i = 0; i < 15; i++)
        {
            float rating = (float)Math.Round(rand.NextDouble() * 10, 1);

            var movie = new Movie(
                idCount++, 
                titles[i], 
                years[i], 
                descriptions[i], 
                rating
            );

            movies.Add(movie);
        }
    }

    public async Task<PagedResult<Movie>> ReadAll(int page, int size)
    {
        int totalCount = movies.Count;
        int start = Math.Clamp((page - 1) * size, 0, totalCount);
        int length = Math.Clamp(size, 0, totalCount - start);
        List<Movie> values = movies.Slice(start, length);
        var pagedResult = new PagedResult<Movie>(values, totalCount);

        return await Task.FromResult(pagedResult);
    }

    public async Task<Movie?> Create(Movie movie)
    {
        movie.Id = idCount++;
        movies.Add(movie);

        return await Task.FromResult(movie);
    }

    public async Task<Movie?> Read(int id)
    {
        Movie? movie = movies.FirstOrDefault((u) => u.Id == id);

        return await Task.FromResult(movie);
    }

    public async Task<Movie?> Update(int id, Movie newMovie)
    {
        Movie? movie = movies.FirstOrDefault((u) => u.Id == id);

        if (movie != null)
        {
            movie.Title = newMovie.Title;
            movie.Year = newMovie.Year;
            movie.Description = newMovie.Description;
            movie.Rating = newMovie.Rating;
        }

        return await Task.FromResult(movie);
    }

    public async Task<Movie?> Delete(int id)
    {
        Movie? movie = movies.FirstOrDefault((u) => u.Id == id);
        if (movie != null)
        {
            movies.Remove(movie);
        }

        return await Task.FromResult(movie);
    }
}
