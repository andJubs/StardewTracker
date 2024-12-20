namespace Stardew_completion_guide_api.Models.DTOs
{
    public class CategoryDto
    {
        public CategoryDto()
        {
            Items = new List<ItemDto>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ItemDto> Items { get; set; }
    }
}
