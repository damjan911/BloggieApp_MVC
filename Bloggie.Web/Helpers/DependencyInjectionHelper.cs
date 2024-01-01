using Bloggie.Web.Data;
using Bloggie.Web.Repositories.Implementations;
using Bloggie.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Helpers
{
	public static class DependencyInjectionHelper
	{
		public static void InjectDbContext(this IServiceCollection services)
		{
			services.AddDbContext<BloggieDbContext>(options => options.UseSqlServer("Server=(localdb)\\MSSQLServer;Database=BloggieDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));
		}

		public static void InjectRepositories(this IServiceCollection services)
		{
			services.AddTransient<ITagRepository,TagRepository>();
			services.AddTransient<IBlogPostRepository,BlogPostRepository>();
		}
	}
}
