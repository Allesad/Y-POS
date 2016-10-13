using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using YumaPos.Common.Infrastructure.Logging;
using YumaPos.FrontEnd.Infrastructure.CommandProcessing;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.API.Models;
using YumaPos.Shared.API.ResponseDtos;
using YumaPos.Shared.Infrastructure;
using Y_POS.Core.Infrastructure.Exceptions;

namespace Y_POS.Core.Infrastructure.Decorators
{
    public sealed class ServerRuntimeErrorDecorator : ITerminalApi
    {
        #region Fields

        private readonly ITerminalApi _actor;
        private readonly ILog _logger;

        #endregion

        #region Constructor

        public ServerRuntimeErrorDecorator(ITerminalApi actor, ILoggingService loggingService)
        {
            if (actor == null) throw new ArgumentNullException(nameof(actor));
            if (loggingService == null) throw new ArgumentNullException(nameof(loggingService));

            _actor = actor;
            _logger = loggingService.GetLog(GetType());

            typeof(ServerRuntimeErrorDecorator).GetRuntimeMethods().First().ReturnType.GenericTypeArguments
        }

        #endregion

        public Task<ResponseDto> AddCashDrawerCheckItem(CashDrawerItemDto itemDto)
        {
            try
            {
                return _actor.AddCashDrawerCheckItem(itemDto);
            }
            catch (FaultException ex)
            {
                _logger.Error("Server error", ex);
                throw new ServerRuntimeException();
            }
        }

        public Task<GuidResponseDto> AddCustomer(CustomerDto customerDto)
        {
            throw new NotImplementedException();
        }

        public Task<IntResponseDto> AddCustomersGroup(CustomerGroupDto customerGroup)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> AddDiscountToOrder(Guid orderId, Guid discountId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> AddDiscountToSplitting(Guid orderId, int splittingNumber, Guid? discountId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderFoodResponseDto> AddGiftCardToOrder(GiftCardOrderItemDto item)
        {
            throw new NotImplementedException();
        }

        public Task<RestaurantOrderResponseDto> AddOrder(Guid orderId, OrderType orderType)
        {
            throw new NotImplementedException();
        }

        public Task<OrderFoodResponseDto> AddOrderItem(RestaurantOrderItemDto item)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> AddRelatedModifierToOrderItem(Guid orderId, OrderItemRelatedModifierDto relatedModifier)
        {
            throw new NotImplementedException();
        }

        public Task<GuidResponseDto> AddTerminalToStore(TerminalDto terminalDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> CashDrawerMakeLoan(PickUpDto pickUpDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> CashDrawerMakePickUp(PickUpDto pickUpDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> ChangePassword(ChangePasswordDto model)
        {
            throw new NotImplementedException();
        }

        public Task<BoolResponseDto> CheckEmployeePin(int pin)
        {
            throw new NotImplementedException();
        }

        public Task<DecimalResponseDto> CheckGiftCardBalance(string cardNumber)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> CheckIn(string cardNo)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> CheckOut(string cardNo)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> ClockOperation(UserActivityType operationType)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> CloseOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<RestaurantOrderListResponseDto> GetActiveOrders()
        {
            throw new NotImplementedException();
        }

        public Task<ActiveOrdersChangesDigestResponseDto> GetActiveOrdersChangesDigest(DateTime timestamp)
        {
            throw new NotImplementedException();
        }

        public Task<FilteredRestaurantOrdersResponseDto> GetActiveOrdersRange(ActiveOrdersFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerGroupListResponseDto> GetAllCustomersGroups()
        {
            throw new NotImplementedException();
        }

        public Task<DiscountListResponseDto> GetAllDiscounts()
        {
            throw new NotImplementedException();
        }

        public Task<GiftCardTypeListResponseDto> GetAllGiftCardTypes()
        {
            throw new NotImplementedException();
        }

        public Task<StoreListResponseDto> GetAllStores()
        {
            throw new NotImplementedException();
        }

        public Task<CashDrawerInfoResponseDto> GetCashDrawerInfoTotal()
        {
            throw new NotImplementedException();
        }

        public Task<CashDrawerItemListResponseDto> GetCashDrawerItems(CashDrawerItemsFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<CheckoutOptionListResponseDto> GetCheckoutOptionsByOrderType(OrderType orderType)
        {
            throw new NotImplementedException();
        }

        public Task<CashDrawerItemResponseDto> GetCurrentCashierLastActivity()
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeResponseDto> GetCurrentEmployee()
        {
            throw new NotImplementedException();
        }

        public Task<StoreResponseDto> GetCurrentStore()
        {
            throw new NotImplementedException();
        }

        public Task<UserClockStateResponseDto> GetCurrentUserClockState()
        {
            throw new NotImplementedException();
        }

        public Task<CustomerResponseDto> GetCustomer(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerAddressListResponseDto> GetCustomerAddresses(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerListResponseDto> GetCustomersByGroup(int groupId, int pageNum, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerListResponseDto> GetCustomersFiltered(CustomerFilterDto filters)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetDefaultImage(ImageSizeType imageSizeType)
        {
            throw new NotImplementedException();
        }

        public Task<FilteredCustomersResponseResponseDto> GetFilteredCustomers(FilteredRequestDto model)
        {
            throw new NotImplementedException();
        }

        public Task<FilteredCustomersOrdersResponseResponseDto> GetFilteredCustomersOrders(FilteredRequestDto model)
        {
            throw new NotImplementedException();
        }

        public Task<CustomersSummaryResponseDto> GetFilteredCustomersSummary(FilteredRequestFilterDto[] model)
        {
            throw new NotImplementedException();
        }

        public Task<RestaurantOrderListResponseDto> GetFilteredOrdersByIds(OrderFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerGroupListResponseDto> GetGroupsWithCustomerAmount()
        {
            throw new NotImplementedException();
        }

        public Task<CashierShiftResponseDto> GetLastCashierShift()
        {
            throw new NotImplementedException();
        }

        public Task<RestaurantOrderResponseDto> GetOrderById(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderFoodListResponseDto> GetOrderFoodItemsByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<RestaurantOrderItemListResponseDto> GetOrderItemsByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<GuidDecimalDictonaryResponseDto> GetOrderItemsCosts(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderMakerResponseDto> GetOrderMaker(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<RestaurantOrderReceiptListResponseDto> GetOrderReceiptsByOrderId(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<RestaurantOrderListResponseDto> GetOrdersByIds(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<OrderStatusListResponseDto> GetOrderStatuses()
        {
            throw new NotImplementedException();
        }

        public Task<OrderTypeListResponseDto> GetOrderTypes()
        {
            throw new NotImplementedException();
        }

        public Task<FilteredRestaurantOrdersResponseDto> GetPagedActiveOrders(OrderFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<FilteredCashDrawerItemResponseDto> GetPagedCashDrawerItems(CashDrawerItemsFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<FilteredRestaurantOrdersResponseDto> GetPagedClosedOrders(OrderFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeResponseDto> GetProfile()
        {
            throw new NotImplementedException();
        }

        public Task<RestaurantOrderReceiptResponseDto> GetReceiptByTransactionId(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<XReportResponseDto> GetReportById(Guid cashDrawerItemId)
        {
            throw new NotImplementedException();
        }

        public Task<XReportResponseDto> GetShiftReport()
        {
            throw new NotImplementedException();
        }

        public Task<LookupListResponseDto> GetSystemSettings(string[] listOfSystemSettings)
        {
            throw new NotImplementedException();
        }

        public Task<TerminalResponseDto> GetTerminal(string terminalId)
        {
            throw new NotImplementedException();
        }

        public Task<TerminalListResponseDto> GetTerminals()
        {
            throw new NotImplementedException();
        }

        public Task<TerminalListResponseDto> GetTerminalsByStoreId(Guid storeId)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerListResponseDto> GetWebCustomers(int pageNum, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<XReportResponseDto> GetXReport()
        {
            throw new NotImplementedException();
        }

        public Task<BoolResponseDto> IsActiveOrdersUpdated(DateTime clientDateTime)
        {
            throw new NotImplementedException();
        }

        public Task<BoolResponseDto> IsMenuUpdated(DateTime clientUtcDateTime)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> MarkActiveOrdersAsRead(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> OverrideLogin(string password, int feature)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> PaymentTransaction(RequestTransactionDto requestTransaction)
        {
            throw new NotImplementedException();
        }

        public Task<OrderFoodResponseDto> RefillGiftCard(GiftCardOrderItemDto item, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> RemoveAllDiscountsFromOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> RemoveCustomerAddress(Guid customerAddressId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> RemoveProductFromOrder(RestaurantOrderItemDto orderItem)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> RemoveRelatedModifierFromOrderItem(OrderItemRelatedModifierDto relatedModifier)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> SavePaymentTransaction(InputTransactionInfoDto data)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> SendReceipt(Guid orderId, string email)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> TryProcessPaymentTransaction(RequestTransactionDto requestTransaction)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateCommonModifierQuantityForOrderItem(OrderItemCommonModifierDto modifier)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateCustomer(CustomerDto customer)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateOrder(RestaurantOrderDto orderDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateOrderCustomer(Guid orderId, Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateOrderItemQuantity(RestaurantOrderItemDto item)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateOrderNote(Guid orderId, string note)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateOrderStatusByOrderId(Guid orderId, int statusId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateProfile(EmployeeDto model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdatePushToken(PushType pushType, string pushToken)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateRelatedModifierQuantityForOrderItem(Guid orderId, OrderItemRelatedModifierDto modifier)
        {
            throw new NotImplementedException();
        }

        public Task<BoolResponseDto> UpdateSplittingsForOrderId(Guid orderId, SplittingType splittingType, string[] parameters)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateTaxExempt(Guid orderId, int splittingNumber, bool isTaxExempt)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateTerminal(TerminalDto terminal)
        {
            throw new NotImplementedException();
        }

        public void SetUserToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task<MenuCacheResponseDto> GetTerminalCacheMenu()
        {
            throw new NotImplementedException();
        }

        public ExecutionContext ExecutionContext {
            get { return _actor.ExecutionContext; }
            set { _actor.ExecutionContext = value; }
        }
    }
}
