using Microsoft.EntityFrameworkCore;
using NoteRush.Data.Models;

namespace NoteRush.Data
{
	public class NoteRushDbContext : DbContext
	{
		protected readonly IConfiguration _configuration;

		public DbSet<User> Users { get; set; }

		public NoteRushDbContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DbConnectionString"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>()
				.HasKey(x => x.Id);

			modelBuilder.Entity<User>()
				.Property(x => x.Email)
				.IsRequired();

			modelBuilder.Entity<User>()
				.Property(x => x.Login)
				.IsRequired()
				.HasMaxLength(32);

			modelBuilder.Entity<User>()
				.Property(x => x.Password)
				.IsRequired();
		}
	}
}