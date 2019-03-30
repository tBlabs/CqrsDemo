using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Storage
{
    public class AppDbContext : DbContext
    {
		public DbSet<Product> Products { get; set; }
		public DbSet<Opinion> Opinions { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDB;Database=test;Trusted_Connection=True;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		//	modelBuilder.Entity<Product>()
		//		.HasKey(x => x.Id);
		}
    }

    public class Opinion	
    {
		public Guid Id { get; set; }
	    public string Text { get; set; }
    }

    public class Product
    {
	    public Guid Id { get; set; }
		public string Name { get; set; }
		public ICollection<Opinion> Opinions { get; set; }
    }
}
