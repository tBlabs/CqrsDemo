using System.Linq;
using Shouldly;
using Xunit;

namespace Storage.Tests
{
	public class Class1
	{
		//[Fact]
		public void Db_connection_test()
		{
			using (var dbContext = new AppDbContext())
			{
				dbContext.Database.EnsureCreated();
				dbContext.Products.Add(new Product { Name = "test" });
				dbContext.SaveChanges();
			}

			using (var dbContext = new AppDbContext())
			{
				dbContext.Products.First().Name.ShouldBe("test");
			}
		}
	}
}
