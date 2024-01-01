using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories.Interfaces
{
	public interface IBlogPostRepository
	{
		Task<BlogPost?> GetByIdAsync(Guid id);
		
		Task<IEnumerable<BlogPost>> GetAllBlogsAsync();

		Task<BlogPost> CreateAsync(BlogPost blogPost);

		Task<BlogPost?> UpdateAsync(BlogPost blogPost);

		Task<BlogPost?> DeleteAsync(Guid id);
	}
}
