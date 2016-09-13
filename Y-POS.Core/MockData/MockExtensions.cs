using System;
using System.Collections.Generic;
using YumaPos.Shared.API.Models;

namespace Y_POS.Core.MockData
{
    public static class MockExtensions
    {
        public static OrderMakerDto ToOrderMakerDto(this RestaurantOrderDto source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new OrderMakerDto
            {
                Title = source.Number.ToString(),
                Status = source.Status,
                IsReadonly = false,
                OrderItems = new[]
                {
                    new OrderFoodDto { Title = "Pokeburger", Quantity = 2, Cost = MockDataGenerator.GetRandomAmount(5, 8),
                        OrderMenuItems = new List<RestaurantMenuItemDto>()},
                    new OrderFoodDto { Title = "Poke Cola", Quantity = 3, Cost = MockDataGenerator.GetRandomAmount(3, 6),
                        OrderMenuItems = new List<RestaurantMenuItemDto>()},
                    new OrderFoodDto { Title = "Poke Whiskey", Quantity = 3, Cost = MockDataGenerator.GetRandomAmount(12, 20),
                        OrderMenuItems = new List<RestaurantMenuItemDto>()}
                }
            };
        }
    }
}
