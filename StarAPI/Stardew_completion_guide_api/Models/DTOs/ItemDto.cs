namespace Stardew_completion_guide_api.Models.DTOs
{
    public class ItemDto
    {
        public ItemDto()
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUri { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string CategoryName { get; set; }
        public Guid CategoryId { get; set; }
    }
}
