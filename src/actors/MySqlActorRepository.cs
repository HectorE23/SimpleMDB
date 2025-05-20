using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleMDB
{
    public class MySqlActorRepository : IActorRepository
    {
        private readonly string connectionString;

        public MySqlActorRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private MySqlConnection OpenDb()
        {
            var dbc = new MySqlConnection(connectionString);
            dbc.Open();
            return dbc;
        }

        public async Task<PagedResult<Actor>> ReadAll(int page, int size)
        {
            using var dbc = OpenDb();

            using var countCmd = dbc.CreateCommand();
            countCmd.CommandText = "SELECT COUNT(*) FROM Actors";
            int totalCount = Convert.ToInt32(await countCmd.ExecuteScalarAsync());

            using var cmd = dbc.CreateCommand();
            cmd.CommandText = "SELECT * FROM Actors LIMIT @limit OFFSET @offset";
            cmd.Parameters.AddWithValue("@limit", size);
            cmd.Parameters.AddWithValue("@offset", (page - 1) * size);

            var actors = new List<Actor>();
            using var rows = await cmd.ExecuteReaderAsync();

            while (await rows.ReadAsync())
            {
                actors.Add(new Actor
                {
                    Id = Convert.ToInt32(rows["id"]),
                    FirstName = rows["first_name"]?.ToString() ?? "",
                    LastName = rows["last_name"]?.ToString() ?? "",
                    Rating = Convert.ToSingle(rows["rating"]),
                    Bio = rows["bio"]?.ToString() ?? ""
                });
            }

            return new PagedResult<Actor>(actors, totalCount);
        }

        public async Task<Actor?> Create(Actor actor)
        {
            using var dbc = OpenDb();

            using var cmd = dbc.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Actors (first_name, last_name, rating, bio) 
                VALUES (@first_name, @last_name, @rating, @bio);
                SELECT LAST_INSERT_ID();";

            cmd.Parameters.AddWithValue("@first_name", actor.FirstName);
            cmd.Parameters.AddWithValue("@last_name", actor.LastName);
            cmd.Parameters.AddWithValue("@rating", actor.Rating);
            cmd.Parameters.AddWithValue("@bio", actor.Bio);

            actor.Id = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            return actor;
        }

        public async Task<Actor?> Read(int id)
        {
            using var dbc = OpenDb();

            using var cmd = dbc.CreateCommand();
            cmd.CommandText = "SELECT * FROM Actors WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            using var rows = await cmd.ExecuteReaderAsync();
            if (await rows.ReadAsync())
            {
                return new Actor
                {
                    Id = Convert.ToInt32(rows["id"]),
                    FirstName = rows["first_name"]?.ToString() ?? "",
                    LastName = rows["last_name"]?.ToString() ?? "",
                    Rating = Convert.ToSingle(rows["rating"]),
                    Bio = rows["bio"]?.ToString() ?? ""
                };
            }

            return null;
        }

        public async Task<Actor?> Update(int id, Actor newActor)
        {
            using var dbc = OpenDb();

            using var cmd = dbc.CreateCommand();
            cmd.CommandText = @"
                UPDATE Actors 
                SET first_name = @first_name, last_name = @last_name, rating = @rating, bio = @bio 
                WHERE id = @id";

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@first_name", newActor.FirstName);
            cmd.Parameters.AddWithValue("@last_name", newActor.LastName);
            cmd.Parameters.AddWithValue("@rating", newActor.Rating);
            cmd.Parameters.AddWithValue("@bio", newActor.Bio);

            int affected = await cmd.ExecuteNonQueryAsync();
            if (affected > 0)
            {
                newActor.Id = id;
                return newActor;
            }

            return null;
        }

        public async Task<Actor?> Delete(int id)
        {
            var actor = await Read(id);
            if (actor == null) return null;

            using var dbc = OpenDb();

            using var cmd = dbc.CreateCommand();
            cmd.CommandText = "DELETE FROM Actors WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            int affected = await cmd.ExecuteNonQueryAsync();
            return affected > 0 ? actor : null;
        }
    }
}
