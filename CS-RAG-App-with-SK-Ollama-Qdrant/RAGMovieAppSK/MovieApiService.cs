namespace RAGMovieAppSK;

public class MovieDetails
{
    public string? Title { get; set; }
    public string[]? Cast { get; set; }
    public string? BoxOffice { get; set; }
    public double Rating { get; set; }
    public string[]? Genre { get; set; }
}

public class MovieApiService
{
    private readonly List<MovieDetails> _movieDetails =
    [
        new()
        {
            Title = "Lion King",
            Cast = ["Matthew Broderick", "James Earl Jones", "Jeremy Irons", "Moira Kelly"],
            BoxOffice = "968.5 million USD",
            Rating = 8.5,
            Genre = ["Animation", "Adventure", "Drama"]
        },

        new()
        {
            Title = "Inception",
            Cast = ["Leonardo DiCaprio", "Joseph Gordon-Levitt", "Elliot Page", "Tom Hardy"],
            BoxOffice = "836.8 million USD",
            Rating = 8.8,
            Genre = ["Action", "Adventure", "Sci-Fi"]
        },

        new()
        {
            Title = "The Matrix",
            Cast = ["Keanu Reeves", "Laurence Fishburne", "Carrie-Anne Moss", "Hugo Weaving"],
            BoxOffice = "463.5 million USD",
            Rating = 8.7,
            Genre = ["Action", "Sci-Fi"]
        },

        new()
        {
            Title = "Shrek",
            Cast = ["Mike Myers", "Eddie Murphy", "Cameron Diaz", "John Lithgow"],
            BoxOffice = "487.9 million USD",
            Rating = 7.9,
            Genre = ["Animation", "Adventure", "Comedy"]
        },

        new()
        {
            Title = "Forrest Gump",
            Cast = ["Tom Hanks", "Robin Wright", "Gary Sinise", "Sally Field"],
            BoxOffice = "678.2 million USD",
            Rating = 8.8,
            Genre = ["Drama", "Romance"]
        },

        new()
        {
            Title = "The Dark Knight",
            Cast = ["Christian Bale", "Heath Ledger", "Aaron Eckhart", "Michael Caine"],
            BoxOffice = "1.005 billion USD",
            Rating = 9.0,
            Genre = ["Action", "Crime", "Drama"]
        },

        new()
        {
            Title = "Pulp Fiction",
            Cast = ["John Travolta", "Uma Thurman", "Samuel L. Jackson", "Bruce Willis"],
            BoxOffice = "213.9 million USD",
            Rating = 8.9,
            Genre = ["Crime", "Drama"]
        },

        new()
        {
            Title = "The Shawshank Redemption",
            Cast = ["Tim Robbins", "Morgan Freeman", "Bob Gunton", "William Sadler"],
            BoxOffice = "58.3 million USD",
            Rating = 9.3,
            Genre = ["Drama"]
        },

        new()
        {
            Title = "Gladiator",
            Cast = ["Russell Crowe", "Joaquin Phoenix", "Connie Nielsen", "Oliver Reed"],
            BoxOffice = "460.5 million USD",
            Rating = 8.5,
            Genre = ["Action", "Adventure", "Drama"]
        },

        new()
        {
            Title = "Titanic",
            Cast = ["Leonardo DiCaprio", "Kate Winslet", "Billy Zane", "Kathy Bates"],
            BoxOffice = "2.195 billion USD",
            Rating = 7.9,
            Genre = ["Drama", "Romance"]
        },

        new()
        {
            Title = "Jurassic Park",
            Cast = ["Sam Neill", "Laura Dern", "Jeff Goldblum", "Richard Attenborough"],
            BoxOffice = "1.046 billion USD",
            Rating = 8.2,
            Genre = ["Adventure", "Sci-Fi", "Thriller"]
        },

        new()
        {
            Title = "The Avengers",
            Cast = ["Robert Downey Jr.", "Chris Evans", "Scarlett Johansson", "Mark Ruffalo"],
            BoxOffice = "1.519 billion USD",
            Rating = 8.0,
            Genre = ["Action", "Adventure", "Sci-Fi"]
        },

        new()
        {
            Title = "Frozen",
            Cast = ["Kristen Bell", "Idina Menzel", "Jonathan Groff", "Josh Gad"],
            BoxOffice = "1.281 billion USD",
            Rating = 7.4,
            Genre = ["Animation", "Adventure", "Comedy"]
        },

        new()
        {
            Title = "Star Wars: A New Hope",
            Cast = ["Mark Hamill", "Harrison Ford", "Carrie Fisher", "Alec Guinness"],
            BoxOffice = "775.4 million USD",
            Rating = 8.6,
            Genre = ["Action", "Adventure", "Fantasy"]
        },

        new()
        {
            Title = "The Lion, the Witch and the Wardrobe",
            Cast = ["Georgie Henley", "Skandar Keynes", "William Moseley", "Anna Popplewell"],
            BoxOffice = "745 million USD",
            Rating = 6.9,
            Genre = ["Adventure", "Family", "Fantasy"]
        },

        new()
        {
            Title = "Avatar",
            Cast = ["Sam Worthington", "Zoe Saldana", "Sigourney Weaver", "Stephen Lang"],
            BoxOffice = "2.923 billion USD",
            Rating = 7.9,
            Genre = ["Action", "Adventure", "Fantasy"]
        },

        new()
        {
            Title = "Harry Potter and the Sorcerer's Stone",
            Cast = ["Daniel Radcliffe", "Rupert Grint", "Emma Watson", "Robbie Coltrane"],
            BoxOffice = "1.006 billion USD",
            Rating = 7.6,
            Genre = ["Adventure", "Family", "Fantasy"]
        },

        new()
        {
            Title = "The Lord of the Rings: The Fellowship of the Ring",
            Cast = ["Elijah Wood", "Ian McKellen", "Viggo Mortensen", "Sean Astin"],
            BoxOffice = "898.2 million USD",
            Rating = 8.8,
            Genre = ["Action", "Adventure", "Drama"]
        },

        new()
        {
            Title = "Toy Story",
            Cast = ["Tom Hanks", "Tim Allen", "Don Rickles", "Jim Varney"],
            BoxOffice = "373.6 million USD",
            Rating = 8.3,
            Genre = ["Animation", "Adventure", "Comedy"]
        },

        new()
        {
            Title = "Deadpool",
            Cast = ["Ryan Reynolds", "Morena Baccarin", "Ed Skrein", "T.J. Miller"],
            BoxOffice = "782.6 million USD",
            Rating = 8.0,
            Genre = ["Action", "Adventure", "Comedy"]
        }
    ];
    public async Task<MovieDetails?> GetMovieDetailsAsync(string title)
    {
        await Task.Delay(50);
        return _movieDetails.FirstOrDefault(m => string.Equals(m.Title, title, StringComparison.OrdinalIgnoreCase));
    }
}