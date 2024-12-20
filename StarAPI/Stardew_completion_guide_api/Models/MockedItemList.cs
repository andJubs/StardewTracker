using Stardew_completion_guide_api.Models.DTOs;

namespace Stardew_completion_guide_api.Models
{
    public static class MockedItemList
    {
        public static List<ItemDto> Items = new List<ItemDto>()
        {
                new () {
                    Id = Guid.Parse("b5409152-542f-421f-a148-fd65d3065890"),
                    Name = "Item number 1",
                    Description = "This is a duck item",
                    ImageUri = "www.google.com.br/ducks.png"
                },
                new () {
                    Id = Guid.Parse("047bd31f-a543-415e-a68c-5e30cbc55d81"),
                    Name = "Item number 2",
                    Description = "This is a frog item",
                    ImageUri = "www.google.com.br/frogs.png"
                },
        };
    }
}
