using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MoviesAPI.Models
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Genre>().Property(g => g.Id).UseIdentityColumn();
		}
		public DbSet<Genre> Genres { get; set; }
		public DbSet<Movie>Movies { get; set; }
	}
}
