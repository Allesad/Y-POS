using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using YumaPos.Client.Services;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.API.Models;
using YumaPos.Shared.API.ResponseDtos;

namespace Y_POS.Core.MockData
{
    public class MockOrderService : IOrderService
    {
        #region Fields

        private readonly List<RestaurantOrderDto> _orders;

        #endregion

        #region Constructor

        public MockOrderService()
        {
            _orders = new List<RestaurantOrderDto>(Enumerable.Range(1, 20).Select(i => new RestaurantOrderDto
            {
                OrderId = Guid.NewGuid(),
                Number = i,
                Amount = MockDataGenerator.GetRandomAmount(10, 40),
                CustomerName = MockDataGenerator.GetRandomCustomerName(),
                Created = MockDataGenerator.GetRandomDate(DateTime.Today, DateTime.Now),
                Status = MockDataGenerator.GetOrderStatus(),
                Type = OrderType.Quick
            }));
        }

        #endregion

        public IObservable<bool> HasNewActiveOrdersSince(DateTime time)
        {
            throw new NotImplementedException();
        }

        public IObservable<T> GetActiveOrders<T>(Func<RestaurantOrderDto, T> mapper)
        {
            throw new NotImplementedException();
        }

        public IObservable<RestaurantOrderDto> GetActiveOrders(int offset, int count, int orderTypeId = -1, string search = null)
        {
            return GetActiveOrdersResponse(offset, count, orderTypeId, search).SelectMany(dto => dto.Results);
        }

        public IObservable<FilteredRestaurantOrdersDto> GetActiveOrdersResponse(int offset = 0, int count = 2147483647, int orderTypeId = -1, string search = null,
            Guid[] preSelectedIds = null)
        {
            var orders = _orders.OrderByDescending(dto => dto.Number).Skip(offset).Take(count).ToArray();
            return Observable.Return(new FilteredRestaurantOrdersDto
            {
                Count = orders.Length,
                Results = orders
            });
        }

        public IObservable<FilteredRestaurantOrdersDto> GetClosedOrders(int offset = 0, int count = 2147483647, DateTime? dateStart = null, DateTime? dateEnd = null,
            string search = null)
        {
            throw new NotImplementedException();
        }

        public IObservable<RestaurantOrderDto> GetOrderById(Guid orderId)
        {
            return Observable.Return(_orders.First(dto => dto.OrderId == orderId));
        }

        public IObservable<RestaurantOrderItemDto[]> GetOrderItemsByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public IObservable<RestaurantOrderItemDto> GetOrderItemForOrderId(Guid orderId, Guid orderItemId)
        {
            throw new NotImplementedException();
        }

        public IObservable<OrderMakerDto> GetOrderMakerById(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public IObservable<Dictionary<Guid, decimal>> GetUpdatedOrderItemsPrices(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public IObservable<IEnumerable<RestaurantOrderDto>> GetOrdersByIds(OrderFilterDto filterDto)
        {
            throw new NotImplementedException();
        }

        public IObservable<IEnumerable<OrderFoodDto>> GetOrderFoodItems(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public IObservable<RestaurantOrderDto> CreateNewOrder(OrderType type)
        {
            var newOrder = new RestaurantOrderDto
            {
                OrderId = Guid.NewGuid(),
                Number = _orders.Count + 1,
                Type = type,
                Status = OrderStatus.New,
                Created = DateTime.Now
            };
            _orders.Add(newOrder);
            return Observable.Return(newOrder);
        }

        public IObservable<OrderFoodDto> AddOrderItem(RestaurantOrderItemDto item)
        {
            throw new NotImplementedException();
        }

        public IObservable<OrderFoodDto> AddGiftCardToOrder(GiftCardOrderItemDto item)
        {
            throw new NotImplementedException();
        }

        public IObservable<OrderFoodDto> AddGiftCardRefillToOrder(GiftCardOrderItemDto item, decimal amount)
        {
            throw new NotImplementedException();
        }

        public IObservable<ResponseDto> UpdateOrderStatus(Guid orderId, int statusId)
        {
            _orders.First(dto => dto.OrderId == orderId).Status = (OrderStatus) statusId;
            return Observable.Return<ResponseDto>(null);
        }

        public IObservable<ResponseDto> UpdateOrderItemQuantity(Guid orderId, Guid orderItemId, int quantity)
        {
            throw new NotImplementedException();
        }

        public IObservable<ResponseDto> UpdateOrderCustomer(Guid orderId, Guid customerId)
        {
            throw new NotImplementedException();
        }

        public IObservable<ResponseDto> CloseOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public IObservable<ResponseDto> RemoveOrderItem(Guid orderId, Guid orderItemId)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> RemoveOrderItemsFromOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        IObservable<Unit> IOrderService.RemoveOrderItemsFromOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
