//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MoviesManager.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBEntities : DbContext
    {
        public DBEntities()
            : base("name=DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Audience> Audiences { get; set; }
        public virtual DbSet<Casting> Castings { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Style> Styles { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
