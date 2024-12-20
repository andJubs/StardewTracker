using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stardew_completion_guide_api.Data;
using Stardew_completion_guide_api.Models;
using Stardew_completion_guide_api.Models.DTOs;
using Stardew_completion_guide_api.Models.Request;
using Stardew_completion_guide_api.Validations;

namespace Stardew_completion_guide_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<CategoryDto>> GetCategories()
        {
            var categories = _context.Categories.Include(c => c.Items).ToList();

            if (categories.Any())
            {
                var categoriesToReturn = categories.Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Items = category.Items.Select(item => new ItemDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description
                    }).ToList()
                }).ToList();

                return Ok(categoriesToReturn);
            }

            return NotFound("No categories found in database.");
        }

        [HttpGet("{Id:Guid}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoryDto> GetCategoryById(Guid Id)
        {
            var category = _context.Categories.Include(c => c.Items).FirstOrDefault(x => x.Id == Id);

            if (category == null)
                return NotFound("Category not found in database!");

            var categoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Items = category.Items.Select(item => new ItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description
                }).ToList()
            };

            return Ok(categoryDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CategoryDto> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var validationResult = new CreateCategoryRequestValidator().Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            return CreatedAtRoute("GetCategoryById", new { Id = newCategory.Id }, newCategory);
        }

        [HttpDelete("{Id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteCategory(Guid Id)
        {
            var category = _context.Categories.Include(c => c.Items).FirstOrDefault(x => x.Id.Equals(Id));

            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{Id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoryDto> UpdateCategory(Guid Id, [FromBody] UpdateCategoryRequest request)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id.Equals(Id));

            if (category == null) return NotFound("Category not found.");

            category.Name = request.Name;
            category.Description = request.Description;

            _context.Categories.Update(category);
            _context.SaveChanges();

            var updatedCategoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Items = category.Items.Select(item => new ItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description
                }).ToList()
            };

            return Ok(updatedCategoryDto);
        }

        [HttpPatch("{Id:Guid}", Name = "PartialCategoryUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PartialUpdateCategory(Guid Id, JsonPatchDocument<CategoryDto> patchDto)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id.Equals(Id));

            if (category == null) return NotFound("Category not found.");

            var categoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            patchDto.ApplyTo(categoryDto, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;

            _context.Categories.Update(category);
            _context.SaveChanges();

            return Ok(categoryDto);
        }
    }
}
