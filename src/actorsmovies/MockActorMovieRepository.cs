using System.Globalization;

namespace SimpleMDB;

public class MockActorMovieRepository : IActorMovieRepository
{
    private IActorRepository actorRepository;
    private IMovieRepository movieRepository;
    private List<ActorMovie> actorMovies;
    private int idCount;

    public MockActorMovieRepository(IActorRepository actorRepository, IMovieRepository movieRepository)
    {
        this.actorRepository = actorRepository;
        this.movieRepository = movieRepository;
        actorMovies = [];
        idCount = 0;
        Random r = new Random();

        for (int aid = 0; aid < 100; aid++)
        {
            int count = r.Next(100);
            for (int j = 0; j < count; j++)
            {
                int mid = r.Next(100);
                actorMovies.Add(new ActorMovie(idCount++, aid, mid, "Popo"));
            }
        }

    }

    public async Task<PagedResult<(ActorMovie, Movie)>> ReadAllMoviesByActor(int actorId, int page, int size)
{
    // Obtener todas las relaciones ActorMovie para el actor
    List<ActorMovie> actorMovieList = actorMovies.FindAll(am => am.ActorId == actorId);

    int totalCount = actorMovieList.Count;

    int start = Math.Clamp((page - 1) * size, 0, totalCount);
    int length = Math.Clamp(size, 0, totalCount - start);

    var pagedActorMovies = actorMovieList.GetRange(start, length);

    List<(ActorMovie, Movie)> values = new();

    // Para cada ActorMovie, obtener la Movie correspondiente
    foreach (var am in pagedActorMovies)
    {
        var movie = await movieRepository.Read(am.MovieId);
        if (movie != null)
        {
            values.Add((am, movie));
        }
    }

    return new PagedResult<(ActorMovie, Movie)>(values, totalCount);
}

public async Task<PagedResult<(ActorMovie, Actor)>> ReadAllActorsByMovie(int movieId, int page, int size)
    {
        List<ActorMovie> ams = actorMovies.FindAll((am) => am.MovieId == movieId);
        List<(ActorMovie, Actor)> actors = [];

        foreach(var am in ams)
        {
            var actor = (await actorRepository.Read(am.ActorId))!;
           actors.Add((am, actor)); 
        }

        int totalCount = actors.Count;
        int start = Math.Clamp((page - 1) * size, 0, totalCount);
        int length = Math.Clamp(size, 0, totalCount - start);
        List<(ActorMovie, Actor)> values = actors.GetRange(start, length);
        var pagedResult = new PagedResult<(ActorMovie, Actor)>(values, totalCount);

        return pagedResult;
    }
    public async Task<List<Actor>> ReadAllActors()
    {
        var pagedResult = await actorRepository.ReadAll(1, int.MaxValue);
        return await Task.FromResult(pagedResult.Values);
    }

    public async Task<List<Movie>> ReadAllMovies()
    {
        var pagedResult = await movieRepository.ReadAll(1, int.MaxValue);
        return pagedResult.Values;
    }
    public async Task<ActorMovie?> Create(int actorId, int movieId, string roleName)
    {
        var actorMovie = new ActorMovie(idCount++, actorId, movieId, roleName);
        actorMovies.Add(actorMovie);
        return await Task.FromResult(actorMovie);
    }
    public async Task<ActorMovie?> Delete(int id)
    {
        ActorMovie? actorMovie = actorMovies.FirstOrDefault((am) => am.Id == id);
        if (actorMovie != null)
        {
            actorMovies.Remove(actorMovie);
        }
        return await Task.FromResult(actorMovie);
    }
}