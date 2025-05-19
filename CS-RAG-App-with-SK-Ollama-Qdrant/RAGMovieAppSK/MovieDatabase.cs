namespace RAGMovieAppSK;

public static class MovieDatabase
{
    public static List<Movie> GetMovies()
    {
        var movieData = new List<Movie>
        {
            new()
            {
                Title = "Lion King",
                Description = "The Lion King is a classic Disney animated film that tells the story of a young lion named Simba who embarks on a journey to reclaim his throne as the king of the Pride Lands after the tragic death of his father.",
                Reference = "https://en.wikipedia.org/wiki/The_Lion_King",
                PublishDate = 1994
            },
            new()
            {
                Title = "Inception",
                Description = "Inception is a science fiction film directed by Christopher Nolan that follows a group of thieves who enter the dreams of their targets to steal information.",
                Reference = "https://en.wikipedia.org/wiki/Inception",
                PublishDate =2010
            },
            new()
            {
                Title = "The Matrix",
                Description = "The Matrix is a science fiction film that follows a computer hacker named Neo who discovers that the world he lives in is a simulated reality created by machines.",
                Reference = "https://en.wikipedia.org/wiki/The_Matrix",
                PublishDate = 1999
            },
            new()
            {
                Title = "Shrek",
                Description = "Shrek is an animated film that tells the story of an ogre named Shrek who embarks on a quest to rescue Princess Fiona from a dragon and bring her back to the kingdom of Duloc.",
                Reference = "https://en.wikipedia.org/wiki/Shrek_(film)",
                PublishDate = 2001
            },
            new()
            {
                Title = "Forrest Gump",
                Description = "Forrest Gump is a drama film that chronicles the life of a kind-hearted man with a low IQ who unwittingly influences several historical events in the 20th century United States.",
                Reference = "https://en.wikipedia.org/wiki/Forrest_Gump",
                PublishDate = 1994
            },
            new()
            {
                Title = "The Dark Knight",
                Description = "The Dark Knight is a superhero film directed by Christopher Nolan featuring Batman as he battles the Joker, a criminal mastermind wreaking havoc on Gotham City.",
                Reference = "https://en.wikipedia.org/wiki/The_Dark_Knight_(film)",
                PublishDate = 2008
            },
            new()
            {
                Title = "Pulp Fiction",
                Description = "Pulp Fiction is a crime film, known for its nonlinear storyline and eclectic dialogue centered around the lives of several criminals in Los Angeles.",
                Reference = "https://en.wikipedia.org/wiki/Pulp_Fiction",
                PublishDate = 1994
            },
            new()
            {
                Title = "The Shawshank Redemption",
                Description = "The Shawshank Redemption is a drama about a banker wrongly convicted of murder who befriends a fellow inmate and finds hope and redemption inside a maximum-security prison.",
                Reference = "https://en.wikipedia.org/wiki/The_Shawshank_Redemption",
                PublishDate = 1994
            },
            new()
            {
                Title = "Gladiator",
                Description = "Gladiator is a historical epic about a Roman general who is betrayed and sold into slavery but rises through the ranks of gladiatorial combat to seek revenge.",
                Reference = "https://en.wikipedia.org/wiki/Gladiator_(2000_film)",
                PublishDate = 2000
            },
            new()
            {
                Title = "Titanic",
                Description = "Titanic is a romantic drama that tells the story of Jack and Rose, two passengers from different social classes who fall in love aboard the ill-fated RMS Titanic.",
                Reference = "https://en.wikipedia.org/wiki/Titanic_(1997_film)",
                PublishDate = 1997
            },
            new()
            {
                Title = "Jurassic Park",
                Description = "Jurassic Park is a science fiction adventure about a theme park where genetically cloned dinosaurs run amok after a security breakdown.",
                Reference = "https://en.wikipedia.org/wiki/Jurassic_Park_(film)",
                PublishDate =1993
            },
            new()
            {
                Title = "The Avengers",
                Description = "The Avengers is a superhero ensemble film where Earth's mightiest heroes team up to stop an alien invasion threatening humanity.",
                Reference = "https://en.wikipedia.org/wiki/The_Avengers_(2012_film)",
                PublishDate = 2012
            },
            new()
            {
                Title = "Frozen",
                Description = "Frozen is an animated musical about two royal sisters in the kingdom of Arendelle, where one sister possesses magical ice powers.",
                Reference = "https://en.wikipedia.org/wiki/Frozen_(2013_film)",
                PublishDate = 2013
            },
            new()
            {
                Title = "Star Wars: A New Hope",
                Description = "Star Wars: A New Hope is the original Star Wars film that follows Luke Skywalker's journey to become a Jedi and fight the evil Galactic Empire.",
                Reference = "https://en.wikipedia.org/wiki/Star_Wars_(film)",
                PublishDate =1977
            },
            new()
            {
                Title = "The Lion, the Witch and the Wardrobe",
                Description = "Based on C.S. Lewis' novel, this fantasy film tells the story of four siblings who enter a magical land through a wardrobe and help defeat an evil witch.",
                Reference = "https://en.wikipedia.org/wiki/The_Chronicles_of_Narnia:_The_Lion,_the_Witch_and_the_Wardrobe",
                PublishDate = 2005
            },
            new()
            {
                Title = "Avatar",
                Description = "Avatar is a science fiction film that follows a paraplegic marine who travels to the alien world of Pandora and becomes involved with its native inhabitants.",
                Reference = "https://en.wikipedia.org/wiki/Avatar_(2009_film)",
                PublishDate = 2009
            },
            new()
            {
                Title = "Harry Potter and the Sorcerer's Stone",
                Description = "This fantasy film chronicles Harry Potter's first year at Hogwarts School of Witchcraft and Wizardry and his discovery of his magical heritage.",
                Reference = "https://en.wikipedia.org/wiki/Harry_Potter_and_the_Philosopher%27s_Stone_(film)",
                PublishDate = 2001
            },
            new()
            {
                Title = "The Lord of the Rings: The Fellowship of the Ring",
                Description = "The first part of an epic fantasy trilogy about a hobbit named Frodo who embarks on a quest to destroy a powerful ring.",
                Reference = "https://en.wikipedia.org/wiki/The_Lord_of_the_Rings:_The_Fellowship_of_the_Ring",
                PublishDate = 2001
            },
            new()
            {
                Title = "Toy Story",
                Description = "Toy Story is an animated film about the secret life of toys when their owners are not around, focusing on the friendship between Woody and Buzz Lightyear.",
                Reference = "https://en.wikipedia.org/wiki/Toy_Story",
                PublishDate = 1995
            },
            new()
            {
                Title = "Deadpool",
                Description = "Deadpool is a superhero film featuring a wisecracking mercenary with accelerated healing powers who seeks revenge on those who experimented on him.",
                Reference = "https://en.wikipedia.org/wiki/Deadpool_(film)",
                PublishDate = 2016
            }
        };
        return movieData;
    }
}