using Microsoft.EntityFrameworkCore;
using NoteRush.Data.Models;

namespace NoteRush.Data
{
	public class NoteRushDbContext : DbContext
	{
		protected readonly IConfiguration _configuration;

		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }

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
				.HasKey(u => u.Id);

			modelBuilder.Entity<User>()
				.Property(u => u.Email)
				.IsRequired();

			modelBuilder.Entity<User>()
				.Property(u => u.Username)
				.IsRequired()
				.HasMaxLength(32);

			modelBuilder.Entity<User>()
				.Property(u => u.Password)
				.IsRequired();

			modelBuilder.Entity<User>()
				.HasOne(u => u.Role)
				.WithMany(r => r.Users)
				.HasForeignKey(u => u.RoleId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Role>()
				.HasKey(r => r.Id);

			modelBuilder.Entity<Role>()
				.Property(r => r.Name)
				.IsRequired();

			modelBuilder.Entity<Role>().HasData(
				new Role { Id = 1, Name = "Administrator" },
				new Role { Id = 2, Name = "User" });
		}
	}
}