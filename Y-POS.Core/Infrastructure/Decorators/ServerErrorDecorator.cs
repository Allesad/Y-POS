using System;
using System.Linq;
using System.Threading.Tasks;
using YumaPos.Shared.Infrastructure;
using YumaPos.FrontEnd.Infrastructure.CommandProcessing;
using YumaPos.Shared.API.Enums;
using YumaPos.Shared.API.Models;
using YumaPos.Shared.API.ResponseDtos;

namespace Y_POS.Core.Infrastructure.Decorators
{
	public sealed class ServerErrorDecorator : ITerminalApi
	{
		private readonly ITerminalApi _actor;

		#region Properties

		public ExecutionContext ExecutionContext {
            get { return _actor.ExecutionContext; }
            set { _actor.ExecutionContext = value; }
        }

		#endregion

		public ServerErrorDecorator(ITerminalApi actor)
		{
			if (actor == null) throw new ArgumentNullException(nameof(actor));
		    _actor = actor;
		}

public Task<ResponseDto> UpdateOrderNote(Guid orderId, String note){}
public Task<ResponseDto> UpdateOrderStatusByOrderId(Guid orderId, Int32 statusId){}
public Task<ResponseDto> UpdateProfile(EmployeeDto model){}
public Task<ResponseDto> UpdatePushToken(PushType pushType, String pushToken){}
public Task<ResponseDto> UpdateRelatedModifierQuantityForOrderItem(Guid orderId, OrderItemRelatedModifierDto modifier){}
public Task<BoolResponseDto> UpdateSplittingsForOrderId(Guid orderId, SplittingType splittingType, String[] parameters){}
public Task<ResponseDto> UpdateTaxExempt(Guid orderId, Int32 splittingNumber, Boolean isTaxExempt){}
public Task<ResponseDto> UpdateTerminal(TerminalDto terminal){}
public void SetUserToken(String token){}
public Task<MenuCacheResponseDto> GetTerminalCacheMenu(){}
public Task<ResponseDto> AddCashDrawerCheckItem(CashDrawerItemDto itemDto){}
public Task<GuidResponseDto> AddCustomer(CustomerDto customerDto){}
public Task<IntResponseDto> AddCustomersGroup(CustomerGroupDto customerGroup){}
public Task<ResponseDto> AddDiscountToOrder(Guid orderId, Guid discountId){}
public Task<ResponseDto> AddDiscountToSplitting(Guid orderId, Int32 splittingNumber, Nullable`1 discountId){}
public Task<OrderFoodResponseDto> AddGiftCardToOrder(GiftCardOrderItemDto item){}
public Task<RestaurantOrderResponseDto> AddOrder(Guid orderId, OrderType orderType){}
public Task<OrderFoodResponseDto> AddOrderItem(RestaurantOrderItemDto item){}
public Task<ResponseDto> AddRelatedModifierToOrderItem(Guid orderId, OrderItemRelatedModifierDto relatedModifier){}
public Task<GuidResponseDto> AddTerminalToStore(TerminalDto terminalDto){}
public Task<ResponseDto> CashDrawerMakeLoan(PickUpDto pickUpDto){}
public Task<ResponseDto> CashDrawerMakePickUp(PickUpDto pickUpDto){}
public Task<ResponseDto> ChangePassword(ChangePasswordDto model){}
public Task<BoolResponseDto> CheckEmployeePin(Int32 pin){}
public Task<DecimalResponseDto> CheckGiftCardBalance(String cardNumber){}
public Task<ResponseDto> CheckIn(String cardNo){}
public Task<ResponseDto> CheckOut(String cardNo){}
public Task<ResponseDto> ClockOperation(UserActivityType operationType){}
public Task<ResponseDto> CloseOrder(Guid orderId){}
public Task<RestaurantOrderListResponseDto> GetActiveOrders(){}
public Task<ActiveOrdersChangesDigestResponseDto> GetActiveOrdersChangesDigest(DateTime timestamp){}
public Task<FilteredRestaurantOrdersResponseDto> GetActiveOrdersRange(ActiveOrdersFilterDto filter){}
public Task<CustomerGroupListResponseDto> GetAllCustomersGroups(){}
public Task<DiscountListResponseDto> GetAllDiscounts(){}
public Task<GiftCardTypeListResponseDto> GetAllGiftCardTypes(){}
public Task<StoreListResponseDto> GetAllStores(){}
public Task<CashDrawerInfoResponseDto> GetCashDrawerInfoTotal(){}
public Task<CashDrawerItemListResponseDto> GetCashDrawerItems(CashDrawerItemsFilterDto filter){}
public Task<CheckoutOptionListResponseDto> GetCheckoutOptionsByOrderType(OrderType orderType){}
public Task<CashDrawerItemResponseDto> GetCurrentCashierLastActivity(){}
public Task<EmployeeResponseDto> GetCurrentEmployee(){}
public Task<StoreResponseDto> GetCurrentStore(){}
public Task<UserClockStateResponseDto> GetCurrentUserClockState(){}
public Task<CustomerResponseDto> GetCustomer(Guid customerId){}
public Task<CustomerAddressListResponseDto> GetCustomerAddresses(Guid customerId){}
public Task<CustomerListResponseDto> GetCustomersByGroup(Int32 groupId, Int32 pageNum, Int32 pageSize){}
public Task<CustomerListResponseDto> GetCustomersFiltered(CustomerFilterDto filters){}
public Task<Byte[]> GetDefaultImage(ImageSizeType imageSizeType){}
public Task<FilteredCustomersResponseResponseDto> GetFilteredCustomers(FilteredRequestDto model){}
public Task<FilteredCustomersOrdersResponseResponseDto> GetFilteredCustomersOrders(FilteredRequestDto model){}
public Task<CustomersSummaryResponseDto> GetFilteredCustomersSummary(FilteredRequestFilterDto[] model){}
public Task<RestaurantOrderListResponseDto> GetFilteredOrdersByIds(OrderFilterDto filter){}
public Task<CustomerGroupListResponseDto> GetGroupsWithCustomerAmount(){}
public Task<CashierShiftResponseDto> GetLastCashierShift(){}
public Task<RestaurantOrderResponseDto> GetOrderById(Guid orderId){}
public Task<OrderFoodListResponseDto> GetOrderFoodItemsByOrderId(Guid orderId){}
public Task<RestaurantOrderItemListResponseDto> GetOrderItemsByOrderId(Guid orderId){}
public Task<GuidDecimalDictonaryResponseDto> GetOrderItemsCosts(Guid orderId){}
public Task<OrderMakerResponseDto> GetOrderMaker(Guid orderId){}
public Task<RestaurantOrderReceiptListResponseDto> GetOrderReceiptsByOrderId(Guid orderId){}
public Task<RestaurantOrderListResponseDto> GetOrdersByIds(Guid[] ids){}
public Task<OrderStatusListResponseDto> GetOrderStatuses(){}
public Task<OrderTypeListResponseDto> GetOrderTypes(){}
public Task<FilteredRestaurantOrdersResponseDto> GetPagedActiveOrders(OrderFilterDto filter){}
public Task<FilteredCashDrawerItemResponseDto> GetPagedCashDrawerItems(CashDrawerItemsFilterDto filter){}
public Task<FilteredRestaurantOrdersResponseDto> GetPagedClosedOrders(OrderFilterDto filter){}
public Task<EmployeeResponseDto> GetProfile(){}
public Task<RestaurantOrderReceiptResponseDto> GetReceiptByTransactionId(Guid transactionId){}
public Task<XReportResponseDto> GetReportById(Guid cashDrawerItemId){}
public Task<XReportResponseDto> GetShiftReport(){}
public Task<LookupListResponseDto> GetSystemSettings(String[] listOfSystemSettings){}
public Task<TerminalResponseDto> GetTerminal(String terminalId){}
public Task<TerminalListResponseDto> GetTerminals(){}
public Task<TerminalListResponseDto> GetTerminalsByStoreId(Guid storeId){}
public Task<CustomerListResponseDto> GetWebCustomers(Int32 pageNum, Int32 pageSize){}
public Task<XReportResponseDto> GetXReport(){}
public Task<BoolResponseDto> IsActiveOrdersUpdated(DateTime clientDateTime){}
public Task<BoolResponseDto> IsMenuUpdated(DateTime clientUtcDateTime){}
public Task<ResponseDto> Logout(){}
public Task<ResponseDto> MarkActiveOrdersAsRead(Guid[] ids){}
public Task<ResponseDto> OverrideLogin(String password, Int32 feature){}
public Task<ResponseDto> PaymentTransaction(RequestTransactionDto requestTransaction){}
public Task<OrderFoodResponseDto> RefillGiftCard(GiftCardOrderItemDto item, Decimal amount){}
public Task<ResponseDto> RemoveAllDiscountsFromOrder(Guid orderId){}
public Task<ResponseDto> RemoveCustomerAddress(Guid customerAddressId){}
public Task<ResponseDto> RemoveProductFromOrder(RestaurantOrderItemDto orderItem){}
public Task<ResponseDto> RemoveRelatedModifierFromOrderItem(OrderItemRelatedModifierDto relatedModifier){}
public Task<ResponseDto> SavePaymentTransaction(InputTransactionInfoDto data){}
public Task<ResponseDto> SendReceipt(Guid orderId, String email){}
public Task<ResponseDto> TryProcessPaymentTransaction(RequestTransactionDto requestTransaction){}
public Task<ResponseDto> UpdateCommonModifierQuantityForOrderItem(OrderItemCommonModifierDto modifier){}
public Task<ResponseDto> UpdateCustomer(CustomerDto customer){}
public Task<ResponseDto> UpdateOrder(RestaurantOrderDto orderDto){}
public Task<ResponseDto> UpdateOrderCustomer(Guid orderId, Guid customerId){}
public Task<ResponseDto> UpdateOrderItemQuantity(RestaurantOrderItemDto item){}
	}
}
