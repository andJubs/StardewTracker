using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Stardew_completion_guide_api.Data;
using Stardew_completion_guide_api.Models;
using Stardew_completion_guide_api.Models.DTOs;
using Stardew_completion_guide_api.Models.Request;
using Stardew_completion_guide_api.Validations;

namespace Stardew_completion_guide_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<ItemDto>> GetItems()
        {
            var itemsToReturn = new List<ItemDto>();
            var items = _context.Items.ToList();

            if (items.Any())
            {
                foreach (var item in items)
                {
                    var category = _context.Categories?.FirstOrDefault(c => c.Id == item.CategoryId);
                    itemsToReturn.Add(new ItemDto()
                    {
                        Id = item.Id,
                        CategoryName = category.Name,
                        Name = item.Name,
                        Description = item.Description,
                        ImageUri = item.ImageUri,
                        IsCompleted = item.IsCompleted,
                        CategoryId = item.CategoryId
                    });
                }
                return itemsToReturn;
            }

            return NotFound("No items found in database.");
        }

        [HttpGet("{Id:Guid}", Name = "GetItemById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ItemDto> GetItemById(Guid Id)
        {
            var foundItem = _context.Items.FirstOrDefault(x => x.Id == Id);

            if (foundItem == null)
                return NotFound("Item not found in database!");

            var category = _context.Categories?.FirstOrDefault(c => c.Id == foundItem.CategoryId);

            var itemDto = new ItemDto()
            {
                Id = foundItem.Id,
                Name = foundItem.Name,
                Description = foundItem.Description,
                ImageUri = foundItem.ImageUri,
                IsCompleted = foundItem.IsCompleted,
                CategoryName = category?.Name,
                CategoryId = foundItem.CategoryId
            };

            return Ok(itemDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ItemDto> CreateItem([FromBody] CreateItemRequest request)
        {
            var validationResult = new CreateItemRequestValidator().Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var category = _context.Categories.FirstOrDefault(x => x.Id == request.CategoryId);

            if (category == null)
                return NotFound("Category not found.");

            var newItem = new Item
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                ImageUri = request.ImageUri,
                IsCompleted = false,
                Category = category,
                CreatedDate = DateTime.UtcNow
            };

            _context.Items.Add(newItem);
            _context.SaveChanges();

            var itemDto = new ItemDto()
            {
                Id = newItem.Id,
                Name = newItem.Name,
                Description = newItem.Description,
                ImageUri = newItem.ImageUri,
                IsCompleted = newItem.IsCompleted,
                CategoryName = category.Name,
                CategoryId = newItem.CategoryId
            };

            return CreatedAtRoute("GetItemById", new { Id = newItem.Id }, itemDto);
        }

        [HttpDelete("{Id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteItem(Guid Id)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id.Equals(Id));

            if (item == null) return NotFound();

            _context.Items.Remove(item);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{Id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ItemDto> UpdateItem(Guid Id, [FromBody] UpdateItemRequest request)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id.Equals(Id));

            if (item == null) return NotFound("Item not found.");

            item.Description = request.Description;
            item.ImageUri = request.ImageUri;
            item.Name = request.Name;

            _context.Items.Update(item);
            _context.SaveChanges();

            var category = _context.Categories?.FirstOrDefault(c => c.Id == item.CategoryId);

            var updatedItemDto = new ItemDto()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ImageUri = item.ImageUri,
                IsCompleted = item.IsCompleted,
                CategoryName = category?.Name,
                CategoryId = item.CategoryId
            };

            return Ok(updatedItemDto);
        }

        [HttpPatch("{Id:Guid}", Name = "PartialItemUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult PartialUpdateItem(Guid Id, JsonPatchDocument<ItemDto> patchDto)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id.Equals(Id));

            if (item == null) return NotFound("Item not found.");

            var itemDto = new ItemDto()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ImageUri = item.ImageUri,
                IsCompleted = item.IsCompleted,
                CategoryId = item.CategoryId
            };

            patchDto.ApplyTo(itemDto, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            item.Name = itemDto.Name;
            item.Description = itemDto.Description;
            item.ImageUri = itemDto.ImageUri;
            item.IsCompleted = itemDto.IsCompleted;

            _context.Items.Update(item);
            _context.SaveChanges();

            return Ok(itemDto);
        }
    }
}
