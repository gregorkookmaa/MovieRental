using System;
using System.Linq;
using MovieRental.Models;

namespace MovieRental.Data
{
    public class DbInitializer
    {

        public static void Initialize(RentingContext context)
        {

            if (context.Clients.Any())
            {
                return;  
            }

            var clients = new Client[]
            {
            new Client{FirstName="Albert",LastName="Kersting",DateOfBirth=DateTime.Parse("1972-06-27"),Phone ="7872806065",Email = "akersting0@macromedia.com",Gender="M",Membership="Basic",AdditionalInfo="Loves Horror movies."},
            new Client{FirstName="Lurleen",LastName="Niland",DateOfBirth=DateTime.Parse("1986-09-13"), Phone ="1303223900",Email = "lniland1@51.la",Gender="M",Membership="None",AdditionalInfo="Wants to find new interesting movies to watch."},
            new Client{FirstName="Hope",LastName="Eaddy",DateOfBirth=DateTime.Parse("1969-04-25"),Phone ="1092930725",Email = "headdy2@google.ca",Gender="F",Membership="Premium",AdditionalInfo="Is new to movies."},
            new Client{FirstName="Jenda",LastName="Kellart",DateOfBirth=DateTime.Parse("1969-01-06"),Phone ="8233799406",Email = "jkellart3@accuweather.com",Gender="F",Membership="Premium",AdditionalInfo="Doesn't like rom-coms or Sci-Fi movies."},
            new Client{FirstName="Rochette",LastName="McClean",DateOfBirth=DateTime.Parse("1989-01-07"),Phone ="7542680605",Email = "rmcclean4@storify.com",Gender="F",Membership="None",AdditionalInfo="Is only interested in action movies."},
            new Client{FirstName="Mort",LastName="Walsh",DateOfBirth=DateTime.Parse("1978-11-10"),Phone ="9813858580",Email = "mwalsh5@google.com",Gender="M",Membership="Basic",AdditionalInfo="Interested in indie movies."},
            new Client{FirstName="Dolores",LastName="Harbidge",DateOfBirth=DateTime.Parse("1999-08-09"),Phone ="3356738574",Email = "dharbidge6@wikispaces.com",Gender="F",Membership="Basic",AdditionalInfo="Bucket list to watch the top 100 rated movies."},
            new Client{FirstName="Ranee",LastName="Gladebeck",DateOfBirth=DateTime.Parse("1993-06-02"),Phone ="5061044611",Email = "rgladebeck7@nationalgeographic.com",Gender="M",Membership="None",AdditionalInfo="Loves Quentin Tarantino Movies."},
            new Client{FirstName="Rufe",LastName="Pound",DateOfBirth=DateTime.Parse("1997-07-08"),Phone ="8364580517",Email = "rpound8@123-reg.co.uk",Gender="M",Membership="None",AdditionalInfo="Prefers romantic dramas."}
            };
            foreach (Client c in clients)
            {
                context.Clients.Add(c);
            }
            context.SaveChanges();

            var rentings = new Renting[]
             {
            new Renting{Date=DateTime.Parse("2022-10-05"),Expires=DateTime.Parse("2022-10-12"),Client = clients[0]},
            new Renting{Date=DateTime.Parse("2022-10-08"),Expires=DateTime.Parse("2022-10-15"),Client = clients[1]},
            new Renting{Date=DateTime.Parse("2022-10-11"),Expires=DateTime.Parse("2022-10-18"),Client = clients[2]},
            new Renting{Date=DateTime.Parse("2022-10-19"),Expires=DateTime.Parse("2022-10-26"),Client = clients[3]},
            new Renting{Date=DateTime.Parse("2022-11-14"),Expires=DateTime.Parse("2022-11-21"),Client = clients[4]},
            new Renting{Date=DateTime.Parse("2022-11-15"),Expires=DateTime.Parse("2022-11-22"),Client = clients[5]},
            new Renting{Date=DateTime.Parse("2022-11-18"),Expires=DateTime.Parse("2022-11-25"),Client = clients[6]},
            new Renting{Date=DateTime.Parse("2022-11-25"),Expires=DateTime.Parse("2022-12-01"),Client = clients[7]},
            new Renting{Date=DateTime.Parse("2022-12-22"),Expires=DateTime.Parse("2022-12-29"),Client = clients[8]},
            new Renting{Date=DateTime.Parse("2022-12-26"),Expires=DateTime.Parse("2023-01-04"),Client = clients[0]},
            new Renting{Date=DateTime.Parse("2023-01-02"),Expires=DateTime.Parse("2023-01-09"),Client = clients[1]},
            new Renting{Date=DateTime.Parse("2023-01-11"),Expires=DateTime.Parse("2023-01-18"),Client = clients[2]},
            new Renting{Date=DateTime.Parse("2023-01-23"),Expires=DateTime.Parse("2023-01-30"),Client = clients[3]},
            new Renting{Date=DateTime.Parse("2023-02-01"),Expires=DateTime.Parse("2023-02-08"),Client = clients[4]},
            new Renting{Date=DateTime.Parse("2023-02-05"),Expires=DateTime.Parse("2023-02-12"),Client = clients[5]},
            new Renting{Date=DateTime.Parse("2023-02-22"),Expires=DateTime.Parse("2023-03-01"),Client = clients[6]},
            new Renting{Date=DateTime.Parse("2023-02-27"),Expires=DateTime.Parse("2023-03-07"),Client = clients[7]},
            };
            foreach (Renting r in rentings)
            {
                context.Rentings.Add(r);
            }
            context.SaveChanges();

            var movies = new Movie[]
            {
            new Movie{Title = "Gladiator",ReleaseDate=DateTime.Parse("2000-06-30"), MovieGenre=MovieGenre.Action},
            new Movie{Title = "Die Hard",ReleaseDate=DateTime.Parse("1988-07-15"), MovieGenre=MovieGenre.Action},
            new Movie{Title = "The Terminator",ReleaseDate=DateTime.Parse("1984-10-26"), MovieGenre=MovieGenre.Action},
            new Movie{Title = "Predator",ReleaseDate=DateTime.Parse("1987-06-12"), MovieGenre=MovieGenre.Action},
            new Movie{Title = "Monty Python and the Holy Grail",ReleaseDate=DateTime.Parse("1975-05-25"), MovieGenre=MovieGenre.Comedy},
            new Movie{Title = "The Big Lebowsky",ReleaseDate=DateTime.Parse("1999-07-09"), MovieGenre=MovieGenre.Comedy},
            new Movie{Title = "Ghostbusters",ReleaseDate=DateTime.Parse("1984-06-07"), MovieGenre=MovieGenre.Comedy},
            new Movie{Title = "Hot Fuzz",ReleaseDate=DateTime.Parse("2007-02-14"), MovieGenre=MovieGenre.Comedy},
            new Movie{Title = "Dune",ReleaseDate=DateTime.Parse("2021-09-03"), MovieGenre=MovieGenre.Sci_fi},
            new Movie{Title = "Star Wars",ReleaseDate=DateTime.Parse("1997-05-30"), MovieGenre=MovieGenre.Sci_fi},
            new Movie{Title = "Star Trek",ReleaseDate=DateTime.Parse("2009-05-08"), MovieGenre=MovieGenre.Sci_fi},
            new Movie{Title = "The Abyss",ReleaseDate=DateTime.Parse("1989-08-09"), MovieGenre=MovieGenre.Sci_fi},
            new Movie{Title = "Aliens",ReleaseDate=DateTime.Parse("1986-07-14"), MovieGenre=MovieGenre.Horror},
            new Movie{Title = "The Endless",ReleaseDate=DateTime.Parse("2017-04-21"), MovieGenre=MovieGenre.Horror},
            new Movie{Title = "The Orphanage",ReleaseDate=DateTime.Parse("2007-10-11"), MovieGenre=MovieGenre.Horror},
            new Movie{Title = "Friday the 13th",ReleaseDate=DateTime.Parse("1980-05-09"), MovieGenre=MovieGenre.Horror},
            new Movie{Title = "Avengers: Endgame",ReleaseDate=DateTime.Parse("2019-04-26"), MovieGenre=MovieGenre.Fantasy},
            new Movie{Title = "Lord of the Rings: The Fellowship of the Ring",ReleaseDate=DateTime.Parse("2002-01-11"), MovieGenre=MovieGenre.Fantasy},
            new Movie{Title = "The Resolution",ReleaseDate=DateTime.Parse("2013-01-25"), MovieGenre=MovieGenre.Thriller},
            };
            foreach (Movie m in movies)
            {
                context.Movies.Add(m);
            }
            context.SaveChanges();

            var rentingMovies = new RentingMovie[]
            {
            new RentingMovie{Renting = rentings[0], Movie = movies[0],ClientRating="No rating"},
            new RentingMovie{Renting = rentings[0], Movie = movies[2],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[0], Movie = movies[7],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[0], Movie = movies[18],ClientRating="⭐⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[0], Movie = movies[11],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[1], Movie = movies[1],ClientRating="⭐⭐"},
            new RentingMovie{Renting = rentings[1], Movie = movies[4],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[1], Movie = movies[8],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[1], Movie = movies[3],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[1], Movie = movies[15],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[2], Movie = movies[0],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[2], Movie = movies[1],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[2], Movie = movies[2],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[2], Movie = movies[18],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[3], Movie = movies[4],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[3], Movie = movies[11],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[3], Movie = movies[12],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[3], Movie = movies[0],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[3], Movie = movies[8],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[4], Movie = movies[7],ClientRating="⭐⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[4], Movie = movies[13],ClientRating="⭐⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[4], Movie = movies[14],ClientRating="⭐⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[4], Movie = movies[17],ClientRating="⭐⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[5], Movie = movies[1],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[5], Movie = movies[4],ClientRating="No rating"},
            new RentingMovie{Renting = rentings[5], Movie = movies[9],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[5], Movie = movies[2],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[5], Movie = movies[15],ClientRating="⭐⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[6], Movie = movies[5],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[6], Movie = movies[8],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[6], Movie = movies[7],ClientRating="⭐⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[6], Movie = movies[10],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[7], Movie = movies[12],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[7], Movie = movies[1],ClientRating="⭐⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[7], Movie = movies[5],ClientRating="⭐⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[7], Movie = movies[16],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[8], Movie = movies[1],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[8], Movie = movies[4],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[8], Movie = movies[8],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[8], Movie = movies[3],ClientRating="⭐⭐"},
            new RentingMovie{Renting = rentings[8], Movie = movies[15],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[9], Movie = movies[6],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[9], Movie = movies[3],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[9], Movie = movies[7],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[9], Movie = movies[18],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[10], Movie = movies[10],ClientRating="No rating"},
            new RentingMovie{Renting = rentings[10], Movie = movies[9],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[10], Movie = movies[3],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[10], Movie = movies[5],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[11], Movie = movies[5],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[11], Movie = movies[13],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[11], Movie = movies[14],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[11], Movie = movies[15],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[11], Movie = movies[0],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[12], Movie = movies[2],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[12], Movie = movies[11],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[12], Movie = movies[17],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[12], Movie = movies[16],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[13], Movie = movies[3],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[13], Movie = movies[7],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[13], Movie = movies[1],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[13], Movie = movies[10],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[13], Movie = movies[13],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[14], Movie = movies[17],ClientRating="⭐⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[14], Movie = movies[5],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[14], Movie = movies[4],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[14], Movie = movies[13],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[15], Movie = movies[0],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[15], Movie = movies[11],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[15], Movie = movies[5],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[15], Movie = movies[7],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[15], Movie = movies[9],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[16], Movie = movies[3],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[16], Movie = movies[11],ClientRating="No rating"},
            new RentingMovie{Renting = rentings[16], Movie = movies[8],ClientRating="⭐⭐⭐⭐"},
            new RentingMovie{Renting = rentings[16], Movie = movies[4],ClientRating="⭐⭐⭐"},
            new RentingMovie{Renting = rentings[16], Movie = movies[17],ClientRating="No rating"},
            };
            foreach (RentingMovie rm in rentingMovies)
            {
                context.RentingMovies.Add(rm);
            }
            context.SaveChanges();
        }
    }
}

