using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
	public class AdminTagsController : Controller
	{
		private readonly ITagRepository _tagRepository;

		public AdminTagsController(ITagRepository tagRepository)
                {
		     _tagRepository = tagRepository;
		}

                [HttpGet]
		public IActionResult Add()
		{
		      return View();
		}

		// For Submit the Form.
		[HttpPost]
		[ActionName("Add")]
		public async Task<IActionResult> Add(AddTagRequest addTagRequest)
		{
		      // Mapping AddTagRequest to Tag Domain Model.
		      var tag = new Tag
		      {
			    Name = addTagRequest.Name,
			    DisplayName = addTagRequest.DisplayName
		      };

		      await _tagRepository.CreateAsync(tag);

		      return RedirectToAction("List");
		}

		[HttpGet]
		[ActionName("List")]
		public async Task<IActionResult> List()
		{
			// use dbContext to read the tags.

			var tags = await _tagRepository.GetAllTagsAsync();

			return View(tags);
		}

		[HttpGet]

		public async Task<IActionResult> Edit(Guid id)
		{
			var tag = await _tagRepository.GetByIdAsync(id);

			if(tag != null)
			{
			     var editTagRequest = new EditTagRequest
			     {
				   Id = tag.Id,
				   Name = tag.Name,
				   DisplayName = tag.DisplayName
			     };

			   return View(editTagRequest);
			}

			return View(null);
		}

		// For Edit the Form
		[HttpPost]
		[ActionName("Edit")]

		public async Task<IActionResult>Edit(EditTagRequest editTagRequest)
		{
			var tag = new Tag 
			{
				Id=editTagRequest.Id,
				Name = editTagRequest.Name, 
				DisplayName = editTagRequest.DisplayName
			};

		    var updatedTag = await _tagRepository.UpdateAsync(tag);

			if (updatedTag != null)
			{
				// Show success notification
				return RedirectToAction("List");

			}
			else
			{
				// show error notification
				return RedirectToAction("Edit", new { id = editTagRequest.Id });
			}

		}

		 [HttpPost]

		public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
		{
			var deletedTag = await _tagRepository.DeleteAsync(editTagRequest.Id);

			if (deletedTag != null)
			{
				// Show success notification
				return RedirectToAction("List");
			}

			// Show an error Notification
			return RedirectToAction("Edit", new {id = editTagRequest.Id});
		 }
	}
}
