namespace Stardew_completion_guide_api.Models.Request
{
    public class CreateItemRequest
    {
        public string? Name { get; set; }
        public string? ImageUri { get; set; }
        public string? Description { get; set; }
        public Guid CategoryId { get; set; }
    }
}
