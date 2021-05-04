using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MoviesManager.Models
{
    public static class DAL
    {
        private static DbContextTransaction Transaction
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return (DbContextTransaction)HttpContext.Current.Session["Transaction"];
                }
                return null;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session["Transaction"] = value;
                }
            }
        }
        private static void BeginTransaction(DBEntities DB)
        {
            if (Transaction != null)
                throw new Exception("Transaction en cours! Impossible d'en démarrer une nouvelle!");
            Transaction = DB.Database.BeginTransaction();
        }
        private static void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
            else
                throw new Exception("Aucune ransaction en cours! Impossible de mettre à jour la base de ddonnées!");
        }

        public static List<ListItem> FilmListItem(this Actor actor)
        {
            List<ListItem> filme = new List<ListItem>();
            if(actor.Castings != null)
            {
                foreach(Casting casting in actor.Castings)
                {
                    filme.Add(new ListItem() { Id = casting.Film.Id, Text = casting.Film.Title });
                }
            }
            return filme;
        }

        public static List<ListItem> FilmListItem(this DBEntities DB)
        {
            List<ListItem> films = new List<ListItem>();
            if (DB.Castings != null)
            {
                foreach (Film film in DB.Films)
                {
                    films.Add(new ListItem() { Id = film.Id, Text = film.Title });
                }
            }
            return films;
        }


        public static List<ListItem> ActorListItem(this Film film)
        {
            List<ListItem> actor = new List<ListItem>();

            if(film.Castings!=null)
            {
                foreach(Casting casting in film.Castings)
                {
                    actor.Add(new ListItem() { Id = casting.Actor.Id, Text = casting.Actor.Name });
                }
            }
            return actor;
        }

        public static List<ListItem> ActorListItem(this DBEntities DB)
        {
            List<ListItem> actors = new List<ListItem>();

            if (DB.Actors != null)
            {
                foreach (Actor actor  in DB.Actors)
                {
                    actors.Add(new ListItem() { Id = actor.Id, Text = actor.Name });
                }
            }
            return actors;
        }

        public static DateTime PostsLastUpdate
        {
            get
            {
                if (HttpRuntime.Cache["PostsUpdate"] == null)
                    HttpRuntime.Cache["PostsUpdate"] = DateTime.Now;
                return (DateTime)HttpRuntime.Cache["PostsUpdate"];
            }
            set
            {
                HttpRuntime.Cache["PostsUpdate"] = value;
            }
        }
        public static bool PostsNeedUpdate(DateTime date)
        {
            return DateTime.Compare(date, PostsLastUpdate) < 0;
        }
        public static UserView ToUserView(this User user)
        {
            return new UserView()
            {
                Id = user.Id,
                AvatarId = user.AvatarId,
                UserName = user.UserName,
                FullName = user.FullName,
                Password = user.Password,
                Admin = user.Admin
            };
        }

        public static bool UserNameExist(this DBEntities DB, string username, int excludedId = 0)
        {
            var users = DB.Users.Where(u => u.UserName.ToLower() == username.Trim().ToLower()).ToList();
            if (users.Count == 0)
                return false;
            else
                if (excludedId != 0 && users.Count == 1)
                return users[0].Id != excludedId;
            return true;
        }

        public static bool PasswordValid(this DBEntities DB, string username, string password)
        {
            return DB.Users.Any(u => (u.UserName == username) && (u.Password == password));
        }

        public static User FindByUserName(this DBEntities DB, string userName)
        {
            User user = DB.Users.Where(u => u.UserName == userName).FirstOrDefault();
            return user;
        }
        public static UserView AddUser(this DBEntities DB, UserView userView)
        {
            userView.SaveAvatar();
            User user = userView.ToUser();
            user = DB.Users.Add(user);
            DB.SaveChanges();
            return user.ToUserView();
        }
        public static bool UpdateUser(this DBEntities DB, UserView userView)
        {
            userView.SaveAvatar();
            User userToUpdate = DB.Users.Find(userView.Id);
            userView.CopyToUser(userToUpdate);
            DB.Entry(userToUpdate).State = EntityState.Modified;
            DB.SaveChanges();
            return true;
        }
        public static bool RemoveUser(this DBEntities DB, UserView userView)
        {
            userView.RemoveAvatar();
            User userToDelete = DB.Users.Find(userView.Id);
            DB.Users.Remove(userToDelete);
            DB.SaveChanges();
            return true;
        }

        public static ActorView ToActorView(this Actor actor)
        {
            return new ActorView
            {
                Id = actor.Id,
                Name = actor.Name,
                Sexe = (SexeType)actor.Sexe,
                BirthDate = actor.BirthDate,
                CountryId = actor.CountryId,
                Country = actor.Country,
                CountryName = (actor.Country != null? actor.Country.Name: ""),
                PhotoId = actor.PhotoId
            };
        }

        public static List<ActorView> ActorViewList(this DBEntities DB)
        {
            List<ActorView> actorViewList = new List<ActorView>();
            foreach(Actor actor in DB.Actors.OrderBy(a => a.Name).ToList())
            {
                actorViewList.Add(actor.ToActorView());
            }
            return actorViewList;
        }

        public static List<string> CountryNameList(this DBEntities DB)
        {
            List<string> countryNameList = new List<string>();
            foreach(Country country in DB.Countries.OrderBy(c => c.Name).ToList())
            {
                countryNameList.Add(country.Name);
            }
            return countryNameList;
        }
        public static string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.Trim().ToLower());
        }
        public static int GetCountryId(this DBEntities DB, string name)
        {
            name = ToTitleCase(name);
            Country country = DB.Countries.Where(c => c.Name == name).FirstOrDefault();
            if (country == null)
            {
                country = DB.Countries.Add(new Country { Id = 0, Name = name });
                DB.SaveChanges();
            }
            return country.Id;
        }

        public static ActorView AddActor(this DBEntities DB, ActorView actorView)
        {
            actorView.SavePhoto();
            Actor actor = actorView.ToActor();
            actor = DB.Actors.Add(actor);
            DB.SaveChanges();
            return actor.ToActorView();
        }

        public static bool UpdateActor(this DBEntities DB,ActorView actorView)
        {
            actorView.SavePhoto();
            Actor actorToUpdate = DB.Actors.Find(actorView.Id);
            actorView.CopyToActor(actorToUpdate);
            DB.Entry(actorToUpdate).State = EntityState.Modified;
            DB.SaveChanges();
            return true;
        }

        public static bool RemoveActor(this DBEntities DB, ActorView actorView)
        {
            actorView.RemovePhoto();
            Actor userToDelete = DB.Actors.Find(actorView.Id);
            DB.Actors.Remove(userToDelete);
            DB.SaveChanges();
            return true;
        }
    }
}