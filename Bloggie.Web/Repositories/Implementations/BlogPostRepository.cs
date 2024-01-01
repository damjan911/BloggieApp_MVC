using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories.Implementations
{
      public class BlogPostRepository : IBlogPostRepository
      {
	    private readonly BloggieDbContext _bloggieDbContext;

	    public BlogPostRepository(BloggieDbContext bloggieDbContext)
            {
		   _bloggieDbContext = bloggieDbContext;
	    }
     
            public async Task<BlogPost> CreateAsync(BlogPost blogPost)
	    {
		   await _bloggieDbContext.AddAsync(blogPost);
		   await _bloggieDbContext.SaveChangesAsync();

		   return blogPost;
	     }

	     public async Task<BlogPost?> DeleteAsync(Guid id)
	     {
		   var existingBlog = await _bloggieDbContext.BlogPosts.FindAsync(id);

		   if (existingBlog != null)
		   {
			 _bloggieDbContext.BlogPosts.Remove(existingBlog);
			 await _bloggieDbContext.SaveChangesAsync();
			 return existingBlog;
		   }

			return null;

	      }

	      public async Task<IEnumerable<BlogPost>> GetAllBlogsAsync()
	      {
		    return await _bloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
	      }

	      public async Task<BlogPost?> GetByIdAsync(Guid id)
	      {
		    return await _bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
	      }

	       public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
	       {
		     var existingBlog = await _bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id.Equals(blogPost.Id));

		     if (existingBlog != null)
		     {
			   existingBlog.Heading = blogPost.Heading;
			   existingBlog.PageTitle = blogPost.PageTitle;
			   existingBlog.Content = blogPost.Content;
			   existingBlog.ShortDescription = blogPost.ShortDescription;
			   existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
			   existingBlog.UrlHandle = blogPost.UrlHandle;
			   existingBlog.PublishedDate = blogPost.PublishedDate;
			   existingBlog.Author = blogPost.Author;
			   existingBlog.Visible = blogPost.Visible;
			   existingBlog.Tags = blogPost.Tags;

			    await _bloggieDbContext.SaveChangesAsync();
			    return existingBlog;
			}

			return null;
		}
	}
}
