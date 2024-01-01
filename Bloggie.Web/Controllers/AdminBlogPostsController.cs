using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Controllers
{
	public class AdminBlogPostsController : Controller
	{
		private readonly ITagRepository _tagRepository;
		private readonly IBlogPostRepository _blogPostRepository;

		public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
			_tagRepository = tagRepository;
			_blogPostRepository = blogPostRepository;
		}

        [HttpGet]
		public async Task<IActionResult> Add()
		{
			// Get all tags from Repository.

			var tags = await _tagRepository.GetAllTagsAsync();

			var model = new AddBlogPostsRequest
			{
				Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
			};
			return View(model);
		}

		[HttpPost]
		[ActionName("Add")]
		public async Task<IActionResult> Add(AddBlogPostsRequest addBlogPostsRequest)
		{
			// Mapping AddBlockPostsRequest to BlockPost Domain Model.
			var blogPost = new BlogPost
			{
				Heading = addBlogPostsRequest.Heading,
				PageTitle = addBlogPostsRequest.PageTitle,
				Content = addBlogPostsRequest.Content,
				ShortDescription = addBlogPostsRequest.ShortDescription,
				FeaturedImageUrl = addBlogPostsRequest.FeaturedImageUrl,
				UrlHandle = addBlogPostsRequest.UrlHandle,
				PublishedDate = addBlogPostsRequest.PublishedDate,
				Author = addBlogPostsRequest.Author,
				Visible = addBlogPostsRequest.Visible
			};

			// Map Tags from selected tags.

			var selectedTags = new List<Tag>();
			foreach (var selectedTagId in addBlogPostsRequest.SelectedTags)
			{
				var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
				var existingTag = await _tagRepository.GetByIdAsync(selectedTagIdAsGuid);

				if (existingTag != null)
				{
					selectedTags.Add(existingTag);
				}
			}

			// Mapping tags back to domain model.

			blogPost.Tags = selectedTags;

			await _blogPostRepository.CreateAsync(blogPost);

			return RedirectToAction("Add");
		}

		[HttpGet]

		public async Task<IActionResult> List()
		{
			// Call the Repository
			var blogPosts = await _blogPostRepository.GetAllBlogsAsync();

			return View(blogPosts);
		}

		[HttpGet]

		public async Task<IActionResult> Edit(Guid id)
		{
			// Retrieve the result from the Repository
			var blogPost = await _blogPostRepository.GetByIdAsync(id);
			var tagsDomainModel = await _tagRepository.GetAllTagsAsync();

			// map the Domain Model into View Model
			if(blogPost != null)
			{
				var editBlogPostRequest = new EditBlogPostsRequest
				{
					Id = blogPost.Id,
					Heading = blogPost.Heading,
					PageTitle = blogPost.PageTitle,
					Content = blogPost.Content,
					ShortDescription = blogPost.ShortDescription,
					FeaturedImageUrl = blogPost.FeaturedImageUrl,
					UrlHandle = blogPost.UrlHandle,
					PublishedDate = blogPost.PublishedDate,
					Author = blogPost.Author,
					Visible = blogPost.Visible,
					Tags = tagsDomainModel.Select(x => new SelectListItem
					{
						Text = x.Name,
						Value = x.Id.ToString()
					}),
					SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
				};

				return View(editBlogPostRequest);
		     
			}

			return View(null);

		}

		// For Edit Form

		[HttpPost]
		[ActionName("Edit")]
		public async Task<IActionResult> Edit(EditBlogPostsRequest editBlogPostsRequest)
		{
			// Map View Model back to Domain Model.
			var blogPost = new BlogPost
			{
				Id = editBlogPostsRequest.Id,
				Heading = editBlogPostsRequest.Heading,
				PageTitle = editBlogPostsRequest.PageTitle,
				Content = editBlogPostsRequest.Content,
				ShortDescription = editBlogPostsRequest.ShortDescription,
				FeaturedImageUrl = editBlogPostsRequest.FeaturedImageUrl,
				UrlHandle = editBlogPostsRequest.UrlHandle,
				PublishedDate = editBlogPostsRequest.PublishedDate,
				Author = editBlogPostsRequest.Author,
				Visible = editBlogPostsRequest.Visible
			};

			// Map Tags into Domain Model
			var selectedTags = new List<Tag>();
			foreach(var selectedTag in editBlogPostsRequest.SelectedTags)
			{
				if(Guid.TryParse(selectedTag, out var tag))
				{
					var foundTag = await _tagRepository.GetByIdAsync(tag);

					if(foundTag != null)
					{
						selectedTags.Add(foundTag);
					}
				}
			}

			blogPost.Tags = selectedTags;

			var updatedBlog = await _blogPostRepository.UpdateAsync(blogPost);

			if(updatedBlog != null)
			{
				return RedirectToAction("Edit");
			}

			return RedirectToAction("Edit");

		}

		[HttpPost]

		public async Task<IActionResult> Delete(EditBlogPostsRequest editBlogPostsRequest)
		{
			// Talk to Repository to delete this Blog Post and tags
			var deletedBlogPost = await _blogPostRepository.DeleteAsync(editBlogPostsRequest.Id);

			if(deletedBlogPost != null)
			{
				// show success notification

				return RedirectToAction("List");
			}

			// show error notification

			return RedirectToAction("Edit", new {id = editBlogPostsRequest.Id});
		}
	}
}

