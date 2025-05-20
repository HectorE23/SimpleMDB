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
            "James", "Mary", "John", "Patricia", "Robert", "Jennifer", "Michael", "Linda",
            "William", "Elizabeth", "David", "Barbara", "Richard", "Susan", "Joseph", "Jessica",
            "Thomas", "Sarah", "Charles", "Karen", "Christopher", "Nancy", "Daniel", "Lisa",
            "Matthew", "Betty", "Anthony", "Margaret", "Mark", "Sandra", "Donald", "Ashley",
            "Steven", "Kimberly", "Paul", "Emily", "Andrew", "Donna", "Joshua", "Michelle",
            "Kenneth", "Dorothy", "Kevin", "Carol", "Brian", "Amanda", "George", "Melissa",
            "Edward", "Deborah", "Ryan", "Sophia", "Brandon", "Olivia", "Aaron", "Chloe",
            "Jason", "Hannah", "Jeffrey", "Ava", "Eric", "Natalie", "Scott", "Grace",
            "Sean", "Victoria", "Justin", "Lily", "Austin", "Zoe", "Bryan", "Hailey",
            "Ethan", "Claire", "Adam", "Samantha", "Patrick", "Ella", "Dylan", "Brooklyn",
            "Zachary", "Avery", "Nathan", "Layla", "Kyle", "Scarlett", "Jeremy", "Stella",
            "Jesse", "Riley", "Jordan", "Aria", "Ian", "Lillian", "Noah", "Mila",
            "Logan", "Aubrey", "Christian", "Nora"
        };

        string[] lastNames = {
            "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis",
            "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas",
            "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White",
            "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson", "Walker", "Young",
            "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
            "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell",
            "Carter", "Roberts", "Phillips", "Evans", "Turner", "Reed", "Morgan", "Bell",
            "Murphy", "Bailey", "Cooper", "Richardson", "Cox", "Howard", "Ward", "Torres",
            "Peterson", "Gray", "Ramsey", "Watson", "Graham", "Foster", "Ortiz", "Sullivan",
            "Bryant", "Alexander", "Russell", "Griffin", "Diaz", "Hayes", "Myers", "Ford",
            "Hamilton", "Gibson", "Ellis", "Reynolds", "Fisher", "Stephens", "Jordan", "Wallace",
            "Wells", "Cole", "Rice", "Powell", "Long", "Perry", "Butler", "Barnes",
            "Brooks", "Ross", "Henderson", "Coleman"
        };

        string[] bios = {
            "Known for captivating drama roles.",
            "Award-winning stage and screen actor.",
            "Famous for blockbuster action films.",
            "Has a rich history in theatre.",
            "Rising star in indie films.",
            "Popular in romantic comedies.",
            "Critically acclaimed performer.",
            "Versatile in both film and television.",
            "Recognized for deep emotional performances.",
            "Loved for comedic timing.",
            "Frequently appears in period dramas.",
            "Notable for strong character work.",
            "Has received numerous film awards.",
            "Starred in major box office hits.",
            "Prominent figure in international cinema.",
            "Started in commercials and became a star.",
            "Former Broadway lead turned film icon.",
            "Bilingual performer loved worldwide.",
            "Voice actor in animated classics.",
            "Breakout star from TV drama.",
            "Also works behind the camera as a director.",
            "Trained in classical Shakespearean acting.",
            "Nominated for multiple Academy Awards.",
            "Acted in award-winning sci-fi films.",
            "Featured in acclaimed crime series.",
            "Known for horror movie roles.",
            "Acted in Oscar-winning foreign films.",
            "Frequent collaborator with top directors.",
            "Versed in martial arts for action roles.",
            "Former model turned successful actor."
        };

        Random rand = new Random();

       for (int i = 0; i < 100; i++)
{
    string firstName = firstNames[i];
    string lastName = lastNames[i];
    string fullName = firstName + " " + lastName;
    string bio = bios[rand.Next(bios.Length)]; // Cambie por bios randoms....
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
