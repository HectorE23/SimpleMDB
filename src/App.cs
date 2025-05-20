using System.Collections;
using System.Net;
using System.Text;
using SimpleMdb;

namespace SimpleMDB;

public class App
{
    private HttpListener server;
    private HttpRouter router;
    private int requestId;

    public App()
    {
        string host = "http://127.0.0.1:8080/";
        server = new HttpListener();
        server.Prefixes.Add(host);

        Console.WriteLine("Server Listening on..." + host);

        var userRepository = new MockUserRepository();
        var userService = new MockUserService(userRepository);
        var userController = new UserController(userService);
        var authController = new AuthController(userService);

        var actorRepository = new MockActorRepository();
        var actorService = new MockActorService(actorRepository);
        var actorController = new ActorController(actorService);

        var movieRepository = new MockMovieRepository();
        var movieService = new MockMovieService(movieRepository);
        var movieController = new MovieController(movieService);

        var actorActorMovieRepository = new MockActorMovieRepository(actorRepository, movieRepository);
        var actorActorMovieService = new MockActorMovieService(actorActorMovieRepository);
        var actorMovieController = new ActorMovieController(actorActorMovieService, actorService, movieService);

        router = new HttpRouter();
        router.Use(HttpUtils.ServeStaticFile);
        router.Use(HttpUtils.ReadRequestFormData);

        router.AddGet("/", authController.LandingPageGet);
        router.AddGet("/register", authController.RegisterGet);
        router.AddPost("/register", authController.RegisterPost);
        router.AddGet("/login", authController.LoginGet);
        router.AddPost("/login", authController.LoginPost);
        router.AddPost("/logout", authController.LogoutPost);

        router.AddGet("/users", authController.CheckAuth, userController.ViewAllUsersGet);
        router.AddGet("/users/add", authController.CheckAdmin, userController.AddUserGet);
        router.AddPost("/users/add", authController.CheckAdmin, userController.AddUserPost);
        router.AddGet("/users/view", authController.CheckAdmin, userController.ViewUserGet);
        router.AddGet("/users/edit", authController.CheckAdmin, userController.EditUserGet);
        router.AddPost("/users/edit", authController.CheckAdmin, userController.EditUserPost);
        router.AddPost("/users/remove", authController.CheckAdmin, userController.RemoveUserPost);

        router.AddGet("/", authController.LandingPageGet);
        router.AddGet("/actors", actorController.ViewAllActorsGet);
        router.AddGet("/actors/add", authController.CheckAuth, actorController.AddActorGet);
        router.AddPost("/actors/add", authController.CheckAuth, actorController.AddActorPost);
        router.AddGet("/actors/view", authController.CheckAuth, actorController.ViewActorGet);
        router.AddGet("/actors/edit", authController.CheckAuth, actorController.EditActorGet);
        router.AddPost("/actors/edit", authController.CheckAuth, actorController.EditActorPost);
        router.AddPost("/actors/remove", authController.CheckAuth, actorController.RemoveActorPost);

        router.AddGet("/", authController.LandingPageGet);
        router.AddGet("/movies", movieController.ViewAllMoviesGet);
        router.AddGet("/movies/add", authController.CheckAuth, movieController.AddMovieGet);
        router.AddPost("/movies/add", authController.CheckAuth, movieController.AddMoviePost);
        router.AddGet("/movies/view", authController.CheckAuth, movieController.ViewMovieGet);
        router.AddGet("/movies/edit", authController.CheckAuth, movieController.EditMovieGet);
        router.AddPost("/movies/edit", authController.CheckAuth, movieController.EditMoviePost);
        router.AddPost("/movies/remove", authController.CheckAuth, movieController.RemoveMoviePost);
        
        router.AddGet("/actors/movies", authController.CheckAuth, actorMovieController.ViewAllMoviesByActor);
        router.AddGet("/actors/movies/add", authController.CheckAuth, actorMovieController.AddMoviesByActorsGet);
        router.AddPost("/actors/movies/add", authController.CheckAuth, actorMovieController.AddMoviesByActorsPost);
        router.AddPost("/actors/movies/remove", authController.CheckAuth, actorMovieController.RemoveMoviesByActorPost);

        router.AddGet("/movies/actors/view", authController.CheckAuth, actorMovieController.ViewAllActorsByMovie);
        router.AddGet("/movies/actors/add", authController.CheckAuth, actorMovieController.AddActorsByMovieGet);
        router.AddPost("/movies/actors/add", authController.CheckAuth, actorMovieController.AddActorsByMoviePost);
        router.AddPost("/movies/actors/remove", authController.CheckAuth, actorMovieController.RemoveActorsByMoviePost);
        
    }

    public async Task Start()
    {
        server.Start();

        while (server.IsListening)
        {
            var ctx = server.GetContext();
            _ = HandleContextAsync(ctx);
        }
    }

    public void Stop()
    {
        server.Stop();
        server.Close();
    }

    private async Task HandleContextAsync(HttpListenerContext ctx)
    {
        var req = ctx.Request;
        var res = ctx.Response;
        var options = new Hashtable();

        var rid = req.Headers["X-Request-ID"] ?? requestId.ToString().PadLeft(6, ' ');
        var method = req.HttpMethod;
        var rawUrl = req.RawUrl;

        res.StatusCode = HttpRouter.RESPONSE_NOT_SENT_YET;
        var startTime = DateTime.UtcNow;
        requestId++;

        string error = "";

        try
        {
            await router.Handle(req, res, options);
        }
        catch (Exception ex)
        {
            error = ex.ToString();

            if (res.StatusCode == HttpRouter.RESPONSE_NOT_SENT_YET)
            {
                if (Environment.GetEnvironmentVariable("DEVELOPMENT_MODE") != "Production")
                {
                    var html = HtmlTemplates.Base("SimpleMDB", "Error Page", ex.ToString());
                    await HttpUtils.Respond(req, res, options, (int)HttpStatusCode.InternalServerError, html);
                }
                else
                {
                    var html = HtmlTemplates.Base("SimpleMDB", "Error Page", "An error occurred.");
                    await HttpUtils.Respond(req, res, options, (int)HttpStatusCode.InternalServerError, html);
                }
            }
        }
        finally
        {
            if (res.StatusCode == HttpRouter.RESPONSE_NOT_SENT_YET)
            {
                var html = HtmlTemplates.Base("SimpleMDB", "Not Found Page", "Resource was not found.");
                await HttpUtils.Respond(req, res, options, (int)HttpStatusCode.NotFound, html);
            }

            var elapsedTime = DateTime.UtcNow - startTime;

            Console.WriteLine($"Request {rid}: {method} {rawUrl} from {req.UserHostName} --> {res.StatusCode} ({res.ContentLength64} bytes) in [{res.ContentType}] in {elapsedTime.TotalMilliseconds} ms error: \"{error}\"");
        }
    }
}
