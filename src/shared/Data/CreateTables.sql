CREATE TABLE IF NOT EXISTS Actors (
    id INT AUTO_INCREMENT PRIMARY KEY,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    rating FLOAT,
    bio TEXT
);

CREATE TABLE IF NOT EXISTS Movies (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255),
    description TEXT,
    release_year INT,
    rating FLOAT
);

CREATE TABLE IF NOT EXISTS ActorMovies (
    id INT AUTO_INCREMENT PRIMARY KEY,
    actor_id INT,
    movie_id INT,
    role_name VARCHAR(100),
    FOREIGN KEY (actor_id) REFERENCES Actors(id) ON DELETE CASCADE,
    FOREIGN KEY (movie_id) REFERENCES Movies(id) ON DELETE CASCADE
);
