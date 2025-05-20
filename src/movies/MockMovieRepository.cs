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
    "The Shawshank Redemption", "Inception", "The Dark Knight", "Pulp Fiction", "Forrest Gump",
    "The Matrix", "Fight Club", "Interstellar", "Gladiator", "The Lord of the Rings: The Fellowship of the Ring",
    "The Godfather", "The Lion King", "Saving Private Ryan", "Jurassic Park", "Titanic",
    "The Shawshank Redemption II", "Inception Returns", "The Dark Knight Reloaded", "Pulp Fiction Rising", "Forrest Gump Origins",
    "The Matrix Final", "Fight Club II", "Interstellar Legacy", "Gladiator III", "The Lord of the Rings: The Fellowship of the Ring Origins",
    "The Godfather II", "The Lion King Returns", "Saving Private Ryan Reloaded", "Jurassic Park Rising", "Titanic Legacy",
    "The Shawshank Redemption III", "Inception Legacy", "The Dark Knight II", "Pulp Fiction Final", "Forrest Gump II",
    "The Matrix II", "Fight Club Final", "Interstellar II", "Gladiator Reloaded", "The Lord of the Rings: The Fellowship of the Ring Final",
    "The Godfather Returns", "The Lion King Legacy", "Saving Private Ryan II", "Jurassic Park II", "Titanic Rising",
    "The Shawshank Redemption Reloaded", "Inception Origins", "The Dark Knight Legacy", "Pulp Fiction II", "Forrest Gump Returns",
    "The Matrix Rising", "Fight Club Returns", "Interstellar Reloaded", "Gladiator Origins", "The Lord of the Rings: The Fellowship of the Ring Reloaded",
    "The Godfather III", "The Lion King II", "Saving Private Ryan Returns", "Jurassic Park III", "Titanic II",
    "The Shawshank Redemption Origins", "Inception II", "The Dark Knight Rising", "Pulp Fiction III", "Forrest Gump Reloaded",
    "The Matrix Origins", "Fight Club III", "Interstellar Origins", "Gladiator Returns", "The Lord of the Rings: The Fellowship of the Ring II",
    "The Godfather Legacy", "The Lion King Origins", "Saving Private Ryan Final", "Jurassic Park Reloaded", "Titanic Final",
    "The Shawshank Redemption Final", "Inception Final", "The Dark Knight III", "Pulp Fiction Returns", "Forrest Gump III",
    "The Matrix III", "Fight Club Origins", "Interstellar Final", "Gladiator Final", "The Lord of the Rings: The Fellowship of the Ring III",
    "The Godfather Origins", "The Lion King Final", "Saving Private Ryan III", "Jurassic Park Legacy", "Titanic Origins",
    "The Shawshank Redemption Legacy", "Inception III", "The Dark Knight Origins", "Pulp Fiction Origins", "Forrest Gump Final",
    "The Matrix Legacy", "Fight Club Legacy", "Interstellar Returns", "Gladiator Rising", "The Lord of the Rings: The Fellowship of the Ring Legacy"
};

int[] years = {
    2000, 1977, 1972, 1970, 2011, 1975, 1973, 1983, 1984, 2009,
    1996, 1971, 1985, 2002, 1981, 1988, 1992, 1980, 1999, 1984,
    2005, 1983, 1976, 1998, 2016, 1978, 1973, 1997, 1974, 2015,
    1989, 1979, 1988, 1980, 1990, 1971, 1982, 1972, 1986, 1981,
    2003, 1997, 1987, 1994, 1995, 1991, 1972, 2014, 1993, 1983,
    2013, 1973, 1976, 1984, 1972, 1975, 1991, 2005, 1976, 1999,
    2009, 1987, 1970, 1996, 2010, 1989, 1985, 1998, 1975, 1978,
    2002, 1984, 1977, 1992, 1972, 1974, 1986, 1970, 2018, 2019,
    1995, 2004, 1988, 1976, 2013, 1993, 1980, 1986, 2012, 2011,
    2020, 1982, 1983, 1971, 1979, 1970, 2001, 1985, 1982, 1974
};

string[] descriptions = {
    "A tale of resilience and hope behind prison walls.",
    "A mind-bending journey through layers of dreams.",
    "A vigilante struggles with justice and chaos in Gotham.",
    "Intersecting lives clash in a web of crime and redemption.",
    "A mans simple life echoes through key moments in history.",
    "A hacker uncovers the shocking truth of his reality.",
    "Two men create a dangerous underground club with a secret purpose.",
    "A mission through space tests humanitys will to survive.",
    "A betrayed general becomes a gladiator to seek revenge.",
    "A band of heroes journey to destroy an ancient evil.",
    "A mafia leader passes the torch in a violent world.",
    "A young lion learns to reclaim his rightful place as king.",
    "Soldiers embark on a perilous mission to save one man.",
    "A park of cloned dinosaurs spirals into chaos.",
    "Love blooms aboard a doomed ocean liner.",
    "Hope thrives as two men plan a daring prison escape.",
    "Dream thieves attempt their most ambitious job yet.",
    "A returning hero faces a new criminal underworld.",
    "Familiar faces return in a stylish crime epic.",
    "The untold story of a Southern icon begins.",
    "The final fight for freedom in a digital world.",
    "Old wounds reopen in the underground fighting scene.",
    "A daughter's journey reveals interstellar secrets.",
    "The arena sees blood once more in a new empire.",
    "An earlier age of Middle-earth rises in conflict.",
    "A new generation continues the mafia legacy.",
    "A future king must face a threat from within.",
    "Brothers-in-arms face betrayal and honor once more.",
    "The park reopens with new nightmares lurking.",
    "A second love story aboard a familiar ship.",
    "A final escape plan challenges everything.",
    "A new dream invades the subconscious world.",
    "A fallen hero rediscovers his purpose.",
    "Final acts bring old stories to conclusion.",
    "An unexpected chapter in a familiar saga.",
    "The matrix is rewritten for the last time.",
    "The club resurfaces in a darker form.",
    "A second journey through time and space.",
    "A legacy of blood and steel continues.",
    "The ring's burden returns in new hands.",
    "A second son rises to power in the family.",
    "A forgotten heir returns to the savannah.",
    "A soldiers journey ends in sacrifice.",
    "Nature strikes again in a wild theme park.",
    "A rekindled romance fights against time.",
    "Old friends reunite for one last stand.",
    "A thief faces the ghost of his past.",
    "A dark knight trains a successor.",
    "A final dance of bullets and betrayal.",
    "A son retraces his fathers footsteps.",
    "A last stand inside a dying system.",
    "Old fists find new battles underground.",
    "A time loop threatens galactic survival.",
    "The arena shakes with revolution.",
    "A secret war rages in the shadows.",
    "Bloodlines resurface in violent power plays.",
    "A legacy is challenged in the wild lands.",
    "Final missions blur the lines of duty.",
    "Chaos erupts as nature reclaims the earth.",
    "A new survivor tells the Titanic story.",
    "A prisoners legend is born anew.",
    "Another dream threatens to fracture reality.",
    "A fallen knight seeks redemption.",
    "A new criminal empire is born.",
    "The legacy of Gump lives on in adventure.",
    "The machine awakens in another timeline.",
    "One last round shakes the fight world.",
    "The stars reveal buried secrets.",
    "The coliseum echoes with familiar screams.",
    "The ring corrupts once again.",
    "The family secrets reach back generations.",
    "Origins of royalty in a fierce kingdom.",
    "One final mission tests loyalty and faith.",
    "New breeds stalk the island once more.",
    "True love resurfaces in icy waters.",
    "The final letter from prison is delivered.",
    "Dreams collide in a desperate finale.",
    "A masked knight confronts his past sins.",
    "The last chapter of pulp and blood.",
    "Destiny catches up with the innocent man.",
    "A glitch brings old threats to life.",
    "A secret truth behind the bruises is revealed.",
    "The universe ends with a sacrifice.",
    "Steel clashes in a new age of betrayal.",
    "A dark fate for the final fellowship.",
    "The godfathers rise from humble beginnings.",
    "A prince defends the savannah one last time.",
    "A last-ditch rescue on enemy soil.",
    "Clones rebel in the final dinosaur chapter.",
    "A forbidden love reawakens beneath the ice.",
    "Letters from a fading prison illuminate truth.",
    "A world crumbles inside the deepest dream.",
    "The cape is passed to a new knight.",
    "Violence, redemption, and resurrection unite.",
    "The Gump family faces a changing world.",
    "Ancient programs rebel against creators.",
    "Warriors return for one last fight.",
    "Wormholes open to a doomed timeline.",
    "The empire’s final breath shakes the world.",
    "A forgotten tale in Middle-earth rises again."
};


        Random rand = new Random();

        for (int i = 0; i < 100; i++)
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
