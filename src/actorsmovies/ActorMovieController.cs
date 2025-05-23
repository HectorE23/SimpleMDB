using System.Collections;
using System.Collections.Specialized;
using System.Net;
using SimpleMDB;

namespace SimpleMdb;

public class ActorMovieController
{
    private IActorMovieService actorMovieService;
    private IActorService actorService;
    private IMovieService movieService;

    public ActorMovieController(IActorMovieService actorMovieService, IActorService actorService, IMovieService movieService)
    {
        this.actorMovieService = actorMovieService;
        this.actorService = actorService;
        this.movieService = movieService;
    }

    public async Task ViewAllMoviesByActor(HttpListenerRequest req, HttpListenerResponse res, Hashtable options)
    {
        string message = req.QueryString["message"] ?? "";

        int aid = int.TryParse(req.QueryString["aid"], out int a) ? a : 1;
        int page = int.TryParse(req.QueryString["page"], out int p) ? p : 1;
        int size = int.TryParse(req.QueryString["size"], out int s) ? s : 5;

        var result1 = await actorService.Read(aid);
        var result2 = await actorMovieService.ReadAllMoviesByActor(aid, page, size);

        if (result1.IsValid && result2.IsValid)
        {
            var actor = result1.Value!;
            var pagedResult = result2.Value!;
            var movies = pagedResult.Values;
            int totalCount = pagedResult.TotalCount;

            string html = ActorMovieHtmlTemplates.ViewAllMoviesByActor(actor, movies, totalCount, page, size);
            string content = HtmlTemplates.Base("SimpleMDB", "View All movies by actor Page", html, message);

            await HttpUtils.Respond(req, res, options, (int)HttpStatusCode.OK, content);
        }
        else
        {
            string error = result1.IsValid ? "" : result1.Error.Message;
            error += result2.IsValid ? "" : result2.Error.Message;
            HttpUtils.AddOptions(options, "redirect", "message", error);
            await HttpUtils.Redirect(req, res, options, "/");
        }
    }

    public async Task ViewAllActorsByMovie(HttpListenerRequest req, HttpListenerResponse res, Hashtable options)
    {
        string message = req.QueryString["message"] ?? "";

        int mid = int.TryParse(req.QueryString["mid"], out int m) ? m : 1;
        int page = int.TryParse(req.QueryString["page"], out int p) ? p : 1;
        int size = int.TryParse(req.QueryString["size"], out int s) ? s : 5;

        var result1 = await movieService.Read(mid);
        var result2 = await actorMovieService.ReadAllActorsByMovie(mid, page, size);

        if (result1.IsValid && result2.IsValid)
        {
            var movie = result1.Value!;
            var pagedResult = result2.Value!;
            var actors = pagedResult.Values;
            int totalCount = pagedResult.TotalCount;

            string html = ActorMovieHtmlTemplates.ViewAllActorsByMovie(movie, actors, totalCount, page, size);
            string content = HtmlTemplates.Base("SimpleMDB", "View All actors by movie Page", html, message);

            await HttpUtils.Respond(req, res, options, (int)HttpStatusCode.OK, content);
        }
        else
        {
            string error = result1.IsValid ? "" : result1.Error.Message;
            error += result2.IsValid ? "" : result2.Error.Message;
            HttpUtils.AddOptions(options, "redirect", "message", error);
            await HttpUtils.Redirect(req, res, options, "/");
        }
    }



    public async Task AddMoviesByActorsGet(HttpListenerRequest req, HttpListenerResponse res, Hashtable options)
    {
        string message = req.QueryString["message"] ?? "";

        int aid = int.TryParse(req.QueryString["aid"], out int a) ? a : 1;

        var result1 = await actorService.Read(aid);
        var result2 = await actorMovieService.ReadAllMovies();

        if (result1.IsValid && result2.IsValid)
        {
            var actor = result1.Value!;
            var movie = result2.Value!;

            string html = ActorMovieHtmlTemplates.AddMoviesByActor(actor, movie);
            string content = HtmlTemplates.Base("SimpleMDB", "Add movies by actor Page", html, message);

            await HttpUtils.Respond(req, res, options, (int)HttpStatusCode.OK, content);
        }
        else
        {
            string error = result1.IsValid ? "" : result1.Error.Message;
            error += result2.IsValid ? "" : result2.Error.Message;
            HttpUtils.AddOptions(options, "redirect", "message", error);
            await HttpUtils.Redirect(req, res, options, "/");
        }
    }

    public async Task AddActorsByMovieGet(HttpListenerRequest req, HttpListenerResponse res, Hashtable options)
    {
        string message = req.QueryString["message"] ?? "";

        int mid = int.TryParse(req.QueryString["mid"], out int m) ? m : -1;

        var result1 = await movieService.Read(mid);
        var result2 = await actorMovieService.ReadAllActors();

        if (result1.IsValid && result2.IsValid)
        {
            var movie = result1.Value!;
            var actors = result2.Value!;

            string html = ActorMovieHtmlTemplates.AddActorsByMovie(movie, actors);
            string content = HtmlTemplates.Base("SimpleMDB", "Add actors by movie Page", html, message);

            await HttpUtils.Respond(req, res, options, (int)HttpStatusCode.OK, content);
        }
        else
        {
            string error = result1.IsValid ? "" : result1.Error.Message;
            error += result2.IsValid ? "" : result2.Error.Message;
            HttpUtils.AddOptions(options, "redirect", "message", error);
            await HttpUtils.Redirect(req, res, options, "/");
        }
    }

    public async Task AddMoviesByActorsPost(HttpListenerRequest req, HttpListenerResponse res, Hashtable options)
    {
        var formData = (NameValueCollection?)options["req.form"] ?? [];
        var aid = int.TryParse(formData["aid"], out int a) ? a : 0;
        var mid = int.TryParse(formData["mid"], out int m) ? m : 1;
        var rolename = formData["rolename"] ?? "Popo";

        if (aid <= 0)
        {
            HttpUtils.AddOptions(options, "redirect", "message", "Invalid actor id.");
            await HttpUtils.Redirect(req, res, options, "/actors/movies");
            return;
        }

        var result = await actorMovieService.Create(aid, mid, rolename);

        if (result.IsValid)
        {
            HttpUtils.AddOptions(options, "redirect", "message", "ActorMovie added succesfully!");

            await HttpUtils.Redirect(req, res, options, $"/actors/movies?aid={aid}");
        }
        else
        {
            HttpUtils.AddOptions(options, "redirect", "message", result.Error!.Message);
            HttpUtils.AddOptions(options, "redirect", formData);

            await HttpUtils.Redirect(req, res, options, $"/actors/movies?add?aid={aid}");
        }
    }

    public async Task AddActorsByMoviePost(HttpListenerRequest req, HttpListenerResponse res, Hashtable options)
    {
        var formData = (NameValueCollection?)options["req.form"] ?? [];
        var aid = int.TryParse(formData["mid"], out int m) ? m : -1;
        var mid = int.TryParse(formData["aid"], out int a) ? a : -1;
        var rolename = formData["rolename"] ?? "Popo";

        if (aid <= 0)
        {
            HttpUtils.AddOptions(options, "redirect", "message", "Invalid actor id.");
            await HttpUtils.Redirect(req, res, options, "/actors/movies");
            return;
        }

        var result = await actorMovieService.Create(aid, mid, rolename);

        if (result.IsValid)
        {
            HttpUtils.AddOptions(options, "redirect", "message", "ActorMovie added succesfully!");

            await HttpUtils.Redirect(req, res, options, $"/movies/actors?aid={mid}");
        }
        else
        {
            HttpUtils.AddOptions(options, "redirect", "message", result.Error!.Message);
            HttpUtils.AddOptions(options, "redirect", formData);

            await HttpUtils.Redirect(req, res, options, $"/movies/actors?add?mid={mid}");
        }
    }

    public async Task RemoveMoviesByActorPost(HttpListenerRequest req, HttpListenerResponse res, Hashtable options)
    {
        string message = req.QueryString["message"] ?? "";

        int amid = int.TryParse(req.QueryString["amid"], out int a) ? a : 1;

        Result<ActorMovie> result = await actorMovieService.Delete(amid);

        if (result.IsValid)
        {
            HttpUtils.AddOptions(options, "redirect", "message", "ActorMovie removed succesfully!");
            await HttpUtils.Redirect(req, res, options, $"/actors/movies?aid={result.Value!.ActorId}");
        }
        else
        {
            HttpUtils.AddOptions(options, "redirect", "message", result.Error!.Message);
            await HttpUtils.Redirect(req, res, options, "/actors");
        }
    }

    public async Task RemoveActorsByMoviePost(HttpListenerRequest req, HttpListenerResponse res, Hashtable options)
    {
        string message = req.QueryString["message"] ?? "";

        int amid = int.TryParse(req.QueryString["amid"], out int a) ? a : 1;

        Result<ActorMovie> result = await actorMovieService.Delete(amid);

        if (result.IsValid)
        {
            HttpUtils.AddOptions(options, "redirect", "message", "ActorMovie removed succesfully!");
            await HttpUtils.Redirect(req, res, options, $"/movies/actors?mid={result.Value!.MovieId}");
        }
        else
        {
            HttpUtils.AddOptions(options, "redirect", "message", result.Error!.Message);
            await HttpUtils.Redirect(req, res, options, "/movies");
        }
    }
}
