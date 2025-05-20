namespace SimpleMDB;

public class ActorMovieHtmlTemplates
{
    public static string ViewAllMoviesByActor(Actor actor, List<(ActorMovie, Movie)> ams, int totalCount, int page, int size)
    {
        int pageCount = (int)Math.Ceiling((double)totalCount / size);
        string rows = "";

        foreach (var (am, movie) in ams)
        {
            rows += @$"
                <tr>
                    <td>{movie.Id}</td>
                    <td>{movie.Title}</td>
                    <td>{movie.Year}</td>
                    <td>{movie.Description}</td>
                    <td>{movie.Rating}</td>
                    <td>{am.RoleName}</td>
                    <td>
                        <form action=""/actors/movies/remove?amid={am.Id}"" method=""POST"" onsubmit=""return confirm('Are you sure that you want to remove this movie from the actor?')"">
                            <input type=""submit"" value=""Remove"">
                        </form>
                    </td>
                </tr>
                ";
        }

        string pDisable = (page > 1).ToString().ToLower();
        string nDisable = (page < pageCount).ToString().ToLower();

        string html = @$"
            <div class=""add"">
                <a href=""/actors/movies/add?aid={actor.Id}"">Add New Movie</a>
            </div>
            <table class=""viewall"">
                <thead>
                    <th>Id</th>
                    <th>Title</th>
                    <th>Year</th>
                    <th>Description</th>
                    <th>Rating</th>
                    <th>Role Name</th>
                    <th>Remove</th>
                </thead>
                <tbody>
                    {rows}
                </tbody>
            </table>
            <div class=""pagination"">
                <a href=""?aid={actor.Id}&page=1&size={size}"" onclick=""return {pDisable};"">First</a>
                <a href=""?aid={actor.Id}&page={page - 1}&size={size}"" onclick=""return {pDisable};"">Prev</a>
                <span>{page} / {pageCount}</span>
                <a href=""?aid={actor.Id}&page={page + 1}&size={size}"" onclick=""return {nDisable};"">Next</a>
                <a href=""?aid={actor.Id}&page={pageCount}&size={size}"" onclick=""return {nDisable};"">Last</a>
            </div>";

        return html;
    }


    public static string ViewAllActorsByMovie(Movie movie, List<(ActorMovie, Actor)> amas, int totalCount, int page, int size)
    {
        int pageCount = (int)Math.Ceiling((double)totalCount / size);
        string rows = "";

        foreach (var (am, actor) in amas)
        {
            rows += @$"
                <tr>
                    <td>{actor.Id}</td>
                    <td>{actor.FirstName}</td>
                    <td>{actor.LastName}</td>
                    <td>{actor.Bio}</td>
                    <td>{actor.Rating}</td>
                     <td>{am.RoleName}</td>
                    <td>
                        <form action=""/movies/actors/remove?amid={am.Id}"" method=""POST"" onsubmit=""return confirm('Are you sure that you want to remove this actor from the movie?')"">
                            <input type=""submit"" value=""Remove"">
                        </form>
                    </td>
                </tr>";
        }

        string pDisable = (page > 1).ToString().ToLower();
        string nDisable = (page < pageCount).ToString().ToLower();

        string html = @$"
            <div class=""add"">
                <a href=""/movies/actors/add?mid={movie.Id}"">Add New ActorMovie</a>
            </div>
            <table class=""viewall"">
                <thead>
                    <th>Id</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Bio</th>
                    <th>Role Name</th>
                    <th>Remove</th>
                </thead>
                <tbody>
                    {rows}
                </tbody>
            </table>
            <div class=""pagination"">
                <a href=""?mid={movie.Id}&page=1&size={size}"" onclick=""return {pDisable};"">First</a>
                <a href=""?mid={movie.Id}&page={page - 1}&size={size}"" onclick=""return {pDisable};"">Prev</a>
                <span>{page} / {pageCount}</span>
                <a href=""?mid={movie.Id}&page={page + 1}&size={size}"" onclick=""return {nDisable};"">Next</a>
                <a href=""?mid={movie.Id}&page={pageCount}&size={size}"" onclick=""return {nDisable};"">Last</a>
            </div>";

        return html;
    }

    public static string AddMoviesByActor(Actor actor, List<Movie> movies)
{
    string movieOptions = "";

    foreach (var movie in movies)
    {
        movieOptions += $@"<option value=""{movie.Id}"">{movie.Title} ({movie.Year})</option>";
    }

    string html = @$"
    <form method=""post"" action=""/actors/movies/add"">
        <input type=""hidden"" name=""aid"" value=""{actor.Id}"" />

        <label for=""actorName"">Actor:</label>
        <input id=""actorName"" type=""text"" value=""{actor.FirstName} {actor.LastName}"" disabled />

        <label for=""mid"">Movies:</label>
        <select name=""mid"" id=""mid"">
            {movieOptions}
        </select>

        <label for=""rolename"">Role Name:</label>
        <input id=""rolename"" name=""rolename"" type=""text"" placeholder=""Role Name"" />

        <input type=""submit"" value=""Add"" />
    </form>
    ";

    return html;
}

public static string AddActorsByMovie(Movie movie, List<Actor> actors)
{
    string actorOptions = "";

    foreach (var actor in actors)
    {
        actorOptions += $@"<option value=""{actor.Id}"">{actor.FirstName} {actor.LastName}</option>";
    }

    string html = @$"
    <form method=""post"" action=""/movies/actors/add"">
        <input type=""hidden"" name=""mid"" value=""{movie.Id}"" />

        <label for=""movieTitle"">Movie:</label>
        <input id=""movieTitle"" type=""text"" value=""{movie.Title} ({movie.Year})"" disabled />

        <label for=""aid"">Actors:</label>
        <select name=""aid"" id=""aid"">
            {actorOptions}
        </select>

        <label for=""rolename"">Role Name:</label>
        <input id=""rolename"" name=""rolename"" type=""text"" placeholder=""Role Name"" />

        <input type=""submit"" value=""Add"" />
    </form>
    ";

    return html;
}

public static string RemoveMoviesByActorPost(int actorId, int movieId)
{
    string html = @$"
    <form method=""post"" action=""/actors/movies/remove"">
        <input type=""hidden"" name=""aid"" value=""{actorId}"" />
        <input type=""hidden"" name=""mid"" value=""{movieId}"" />
        <input type=""submit"" value=""Remove"" onclick=""return confirm('Are you sure you want to remove this movie from the actor?');"" />
    </form>
    ";

    return html;
}
}
