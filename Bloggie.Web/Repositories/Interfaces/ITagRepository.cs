using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories.Interfaces
{
	public interface ITagRepository
	{
		Task<Tag?> GetByIdAsync(Guid id);
		
		Task<IEnumerable<Tag>> GetAllTagsAsync();

		Task<Tag> CreateAsync(Tag tag);

		Task<Tag?> UpdateAsync(Tag tag);

		Task<Tag?> DeleteAsync(Guid id);

	}
}
