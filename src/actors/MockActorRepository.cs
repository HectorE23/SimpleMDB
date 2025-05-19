namespace SimpleMDB;

public class MockActorRepository : IActorRepository
{
    private List<Actor> actors;
    private int idCount;

    public MockActorRepository()
    {
        actors = new List<Actor>();
        idCount = 0;

        string[] firstNames = {
            "Ana", "Andres", "Alice", "Benjamin", 
            "Beatriz", "Carlos", "Carla", "Daniel", 
            "Diana", "Emma", "Elena", "Fernando",
            "Amelia", "Brian", "Camila"
        };

        string[] lastNames = {
            "Smith", "Gonzalez", "Johnson", "Martinez", 
            "Brown", "Hernandez", "Davis", "Ramirez", 
            "Miller", "Lopez", "Wilson", "Torres",
            "Taylor", "Garcia", "Perez"
        };

        string[] bios = {
            "Ana is a rising star in independent cinema.",
            "Andres has appeared in multiple drama series.",
            "Alice is known for her comedic timing.",
            "Benjamin has worked in both theater and film.",
            "Beatriz is a versatile actress from Spain.",
            "Carlos has a background in classical acting.",
            "Carla is famous for romantic comedies.",
            "Daniel starred in a major historical film.",
            "Diana is a talented actress and producer.",
            "Emma began her career in indie movies.",
            "Elena is recognized for her emotional range.",
            "Fernando is well-known in Latin American cinema.",
            "Amelia shines in period dramas.",
            "Brian has received multiple award nominations.",
            "Camila is a fan favorite in fantasy films."
        };

        Random rand = new Random();

        for (int i = 0; i < 15; i++)
        {
            string firstName = firstNames[i];
            string lastName = lastNames[i];
            string fullName = firstName + " " + lastName;
            string bio = bios[i];
            float rating = (float)Math.Round(rand.NextDouble() * 10, 1);

            var actor = new Actor(idCount++, firstName, lastName, bio, rating);

            actors.Add(actor);
        }
    }

    public async Task<PagedResult<Actor>> ReadAll(int page, int size)
    {
        int totalCount = actors.Count;
        int start = Math.Clamp((page - 1) * size, 0, totalCount);
        int length = Math.Clamp(size, 0, totalCount - start);
        List<Actor> values = actors.Slice(start, length);
        var pagedResult = new PagedResult<Actor>(values, totalCount);

        return await Task.FromResult(pagedResult);
    }

    public async Task<Actor?> Create(Actor actor)
    {
        actor.Id = idCount++;
        actors.Add(actor);

        return await Task.FromResult(actor);
    }

    public async Task<Actor?> Read(int id)
    {
        Actor? actor = actors.FirstOrDefault((u) => u.Id == id);

        return await Task.FromResult(actor);
    }

    public async Task<Actor?> Update(int id, Actor newActor)
    {
        Actor? actor = actors.FirstOrDefault((u) => u.Id == id);

        if (actor != null)
        {
            actor.FirstName = newActor.FirstName;
            actor.LastName = newActor.LastName;
            actor.Bio = newActor.Bio;
            actor.Rating = newActor.Rating;
        }

        return await Task.FromResult(actor);
    }

    public async Task<Actor?> Delete(int id)
    {
        Actor? actor = actors.FirstOrDefault((u) => u.Id == id);
        if (actor != null)
        {
            actors.Remove(actor);
        }

        return await Task.FromResult(actor);
    }
}
