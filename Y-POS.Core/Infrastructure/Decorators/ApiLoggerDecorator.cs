using System;
using YumaPos.Shared.Infrastructure;
using YumaPos.FrontEnd.Infrastructure.CommandProcessing;

namespace Y_POS.Core.Infrastructure.Decorators
{
	public sealed class ApiLoggerDecorator : ITerminalApi
	{
		private readonly ITerminalApi _actor;

		#region Properties

		public ExecutionContext ExecutionContext {
            get { return _actor.ExecutionContext; }
            set { _actor.ExecutionContext = value; }
        }

		#endregion

		public ApiLoggerDecorator(ITerminalApi actor)
		{
			if (actor == null) throw new ArgumentNullException(nameof(actor));

		    _actor = actor;
		}

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateOrderNote(System.Guid orderId, System.String note)
         {
using (TimeLogger.GetTimeLogger("UpdateOrderNote"))
{
return await _actor.UpdateOrderNote(orderId, note);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateOrderStatusByOrderId(System.Guid orderId, System.Int32 statusId)
         {
using (TimeLogger.GetTimeLogger("UpdateOrderStatusByOrderId"))
{
return await _actor.UpdateOrderStatusByOrderId(orderId, statusId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateProfile(YumaPos.Shared.API.Models.EmployeeDto model)
         {
using (TimeLogger.GetTimeLogger("UpdateProfile"))
{
return await _actor.UpdateProfile(model);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdatePushToken(YumaPos.Shared.API.Enums.PushType pushType, System.String pushToken)
         {
using (TimeLogger.GetTimeLogger("UpdatePushToken"))
{
return await _actor.UpdatePushToken(pushType, pushToken);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateRelatedModifierQuantityForOrderItem(System.Guid orderId, YumaPos.Shared.API.Models.OrderItemRelatedModifierDto modifier)
         {
using (TimeLogger.GetTimeLogger("UpdateRelatedModifierQuantityForOrderItem"))
{
return await _actor.UpdateRelatedModifierQuantityForOrderItem(orderId, modifier);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.BoolResponseDto> UpdateSplittingsForOrderId(System.Guid orderId, YumaPos.Shared.API.Enums.SplittingType splittingType, System.String[] parameters)
         {
using (TimeLogger.GetTimeLogger("UpdateSplittingsForOrderId"))
{
return await _actor.UpdateSplittingsForOrderId(orderId, splittingType, parameters);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateTaxExempt(System.Guid orderId, System.Int32 splittingNumber, System.Boolean isTaxExempt)
         {
using (TimeLogger.GetTimeLogger("UpdateTaxExempt"))
{
return await _actor.UpdateTaxExempt(orderId, splittingNumber, isTaxExempt);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateTerminal(YumaPos.Shared.API.Models.TerminalDto terminal)
         {
using (TimeLogger.GetTimeLogger("UpdateTerminal"))
{
return await _actor.UpdateTerminal(terminal);
}
         }

         public async void SetUserToken(System.String token)
         {
            _actor.SetUserToken(token);
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.MenuCacheResponseDto> GetTerminalCacheMenu()
         {
using (TimeLogger.GetTimeLogger("GetTerminalCacheMenu"))
{
return await _actor.GetTerminalCacheMenu();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> AddCashDrawerCheckItem(YumaPos.Shared.API.Models.CashDrawerItemDto itemDto)
         {
using (TimeLogger.GetTimeLogger("AddCashDrawerCheckItem"))
{
return await _actor.AddCashDrawerCheckItem(itemDto);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.GuidResponseDto> AddCustomer(YumaPos.Shared.API.Models.CustomerDto customerDto)
         {
using (TimeLogger.GetTimeLogger("AddCustomer"))
{
return await _actor.AddCustomer(customerDto);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.IntResponseDto> AddCustomersGroup(YumaPos.Shared.API.Models.CustomerGroupDto customerGroup)
         {
using (TimeLogger.GetTimeLogger("AddCustomersGroup"))
{
return await _actor.AddCustomersGroup(customerGroup);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> AddDiscountToOrder(System.Guid orderId, System.Guid discountId)
         {
using (TimeLogger.GetTimeLogger("AddDiscountToOrder"))
{
return await _actor.AddDiscountToOrder(orderId, discountId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> AddDiscountToSplitting(System.Guid orderId, System.Int32 splittingNumber, Guid? discountId)
         {
using (TimeLogger.GetTimeLogger("AddDiscountToSplitting"))
{
return await _actor.AddDiscountToSplitting(orderId, splittingNumber, discountId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderFoodResponseDto> AddGiftCardToOrder(YumaPos.Shared.API.Models.GiftCardOrderItemDto item)
         {
using (TimeLogger.GetTimeLogger("AddGiftCardToOrder"))
{
return await _actor.AddGiftCardToOrder(item);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderResponseDto> AddOrder(System.Guid orderId, YumaPos.Shared.API.Enums.OrderType orderType)
         {
using (TimeLogger.GetTimeLogger("AddOrder"))
{
return await _actor.AddOrder(orderId, orderType);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderFoodResponseDto> AddOrderItem(YumaPos.Shared.API.Models.RestaurantOrderItemDto item)
         {
using (TimeLogger.GetTimeLogger("AddOrderItem"))
{
return await _actor.AddOrderItem(item);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> AddRelatedModifierToOrderItem(System.Guid orderId, YumaPos.Shared.API.Models.OrderItemRelatedModifierDto relatedModifier)
         {
using (TimeLogger.GetTimeLogger("AddRelatedModifierToOrderItem"))
{
return await _actor.AddRelatedModifierToOrderItem(orderId, relatedModifier);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.GuidResponseDto> AddTerminalToStore(YumaPos.Shared.API.Models.TerminalDto terminalDto)
         {
using (TimeLogger.GetTimeLogger("AddTerminalToStore"))
{
return await _actor.AddTerminalToStore(terminalDto);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> CashDrawerMakeLoan(YumaPos.Shared.API.Models.PickUpDto pickUpDto)
         {
using (TimeLogger.GetTimeLogger("CashDrawerMakeLoan"))
{
return await _actor.CashDrawerMakeLoan(pickUpDto);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> CashDrawerMakePickUp(YumaPos.Shared.API.Models.PickUpDto pickUpDto)
         {
using (TimeLogger.GetTimeLogger("CashDrawerMakePickUp"))
{
return await _actor.CashDrawerMakePickUp(pickUpDto);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> ChangePassword(YumaPos.Shared.API.Models.ChangePasswordDto model)
         {
using (TimeLogger.GetTimeLogger("ChangePassword"))
{
return await _actor.ChangePassword(model);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.BoolResponseDto> CheckEmployeePin(System.Int32 pin)
         {
using (TimeLogger.GetTimeLogger("CheckEmployeePin"))
{
return await _actor.CheckEmployeePin(pin);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.DecimalResponseDto> CheckGiftCardBalance(System.String cardNumber)
         {
using (TimeLogger.GetTimeLogger("CheckGiftCardBalance"))
{
return await _actor.CheckGiftCardBalance(cardNumber);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> CheckIn(System.String cardNo)
         {
using (TimeLogger.GetTimeLogger("CheckIn"))
{
return await _actor.CheckIn(cardNo);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> CheckOut(System.String cardNo)
         {
using (TimeLogger.GetTimeLogger("CheckOut"))
{
return await _actor.CheckOut(cardNo);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> ClockOperation(YumaPos.Shared.API.Enums.UserActivityType operationType)
         {
using (TimeLogger.GetTimeLogger("ClockOperation"))
{
return await _actor.ClockOperation(operationType);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> CloseOrder(System.Guid orderId)
         {
using (TimeLogger.GetTimeLogger("CloseOrder"))
{
return await _actor.CloseOrder(orderId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderListResponseDto> GetActiveOrders()
         {
using (TimeLogger.GetTimeLogger("GetActiveOrders"))
{
return await _actor.GetActiveOrders();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ActiveOrdersChangesDigestResponseDto> GetActiveOrdersChangesDigest(System.DateTime timestamp)
         {
using (TimeLogger.GetTimeLogger("GetActiveOrdersChangesDigest"))
{
return await _actor.GetActiveOrdersChangesDigest(timestamp);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredRestaurantOrdersResponseDto> GetActiveOrdersRange(YumaPos.Shared.API.Models.ActiveOrdersFilterDto filter)
         {
using (TimeLogger.GetTimeLogger("GetActiveOrdersRange"))
{
return await _actor.GetActiveOrdersRange(filter);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerGroupListResponseDto> GetAllCustomersGroups()
         {
using (TimeLogger.GetTimeLogger("GetAllCustomersGroups"))
{
return await _actor.GetAllCustomersGroups();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.DiscountListResponseDto> GetAllDiscounts()
         {
using (TimeLogger.GetTimeLogger("GetAllDiscounts"))
{
return await _actor.GetAllDiscounts();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.GiftCardTypeListResponseDto> GetAllGiftCardTypes()
         {
using (TimeLogger.GetTimeLogger("GetAllGiftCardTypes"))
{
return await _actor.GetAllGiftCardTypes();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.StoreListResponseDto> GetAllStores()
         {
using (TimeLogger.GetTimeLogger("GetAllStores"))
{
return await _actor.GetAllStores();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CashDrawerInfoResponseDto> GetCashDrawerInfoTotal()
         {
using (TimeLogger.GetTimeLogger("GetCashDrawerInfoTotal"))
{
return await _actor.GetCashDrawerInfoTotal();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CashDrawerItemListResponseDto> GetCashDrawerItems(YumaPos.Shared.API.Models.CashDrawerItemsFilterDto filter)
         {
using (TimeLogger.GetTimeLogger("GetCashDrawerItems"))
{
return await _actor.GetCashDrawerItems(filter);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CheckoutOptionListResponseDto> GetCheckoutOptionsByOrderType(YumaPos.Shared.API.Enums.OrderType orderType)
         {
using (TimeLogger.GetTimeLogger("GetCheckoutOptionsByOrderType"))
{
return await _actor.GetCheckoutOptionsByOrderType(orderType);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CashDrawerItemResponseDto> GetCurrentCashierLastActivity()
         {
using (TimeLogger.GetTimeLogger("GetCurrentCashierLastActivity"))
{
return await _actor.GetCurrentCashierLastActivity();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.EmployeeResponseDto> GetCurrentEmployee()
         {
using (TimeLogger.GetTimeLogger("GetCurrentEmployee"))
{
return await _actor.GetCurrentEmployee();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.StoreResponseDto> GetCurrentStore()
         {
using (TimeLogger.GetTimeLogger("GetCurrentStore"))
{
return await _actor.GetCurrentStore();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.UserClockStateResponseDto> GetCurrentUserClockState()
         {
using (TimeLogger.GetTimeLogger("GetCurrentUserClockState"))
{
return await _actor.GetCurrentUserClockState();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerResponseDto> GetCustomer(System.Guid customerId)
         {
using (TimeLogger.GetTimeLogger("GetCustomer"))
{
return await _actor.GetCustomer(customerId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerAddressListResponseDto> GetCustomerAddresses(System.Guid customerId)
         {
using (TimeLogger.GetTimeLogger("GetCustomerAddresses"))
{
return await _actor.GetCustomerAddresses(customerId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerListResponseDto> GetCustomersByGroup(System.Int32 groupId, System.Int32 pageNum, System.Int32 pageSize)
         {
using (TimeLogger.GetTimeLogger("GetCustomersByGroup"))
{
return await _actor.GetCustomersByGroup(groupId, pageNum, pageSize);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerListResponseDto> GetCustomersFiltered(YumaPos.Shared.API.Models.CustomerFilterDto filters)
         {
using (TimeLogger.GetTimeLogger("GetCustomersFiltered"))
{
return await _actor.GetCustomersFiltered(filters);
}
         }

         public async System.Threading.Tasks.Task<System.Byte[]> GetDefaultImage(YumaPos.Shared.API.Enums.ImageSizeType imageSizeType)
         {
using (TimeLogger.GetTimeLogger("GetDefaultImage"))
{
return await _actor.GetDefaultImage(imageSizeType);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredCustomersResponseResponseDto> GetFilteredCustomers(YumaPos.Shared.API.Models.FilteredRequestDto model)
         {
using (TimeLogger.GetTimeLogger("GetFilteredCustomers"))
{
return await _actor.GetFilteredCustomers(model);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredCustomersOrdersResponseResponseDto> GetFilteredCustomersOrders(YumaPos.Shared.API.Models.FilteredRequestDto model)
         {
using (TimeLogger.GetTimeLogger("GetFilteredCustomersOrders"))
{
return await _actor.GetFilteredCustomersOrders(model);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomersSummaryResponseDto> GetFilteredCustomersSummary(YumaPos.Shared.API.Models.FilteredRequestFilterDto[] model)
         {
using (TimeLogger.GetTimeLogger("GetFilteredCustomersSummary"))
{
return await _actor.GetFilteredCustomersSummary(model);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderListResponseDto> GetFilteredOrdersByIds(YumaPos.Shared.API.Models.OrderFilterDto filter)
         {
using (TimeLogger.GetTimeLogger("GetFilteredOrdersByIds"))
{
return await _actor.GetFilteredOrdersByIds(filter);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerGroupListResponseDto> GetGroupsWithCustomerAmount()
         {
using (TimeLogger.GetTimeLogger("GetGroupsWithCustomerAmount"))
{
return await _actor.GetGroupsWithCustomerAmount();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CashierShiftResponseDto> GetLastCashierShift()
         {
using (TimeLogger.GetTimeLogger("GetLastCashierShift"))
{
return await _actor.GetLastCashierShift();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderResponseDto> GetOrderById(System.Guid orderId)
         {
using (TimeLogger.GetTimeLogger("GetOrderById"))
{
return await _actor.GetOrderById(orderId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderFoodListResponseDto> GetOrderFoodItemsByOrderId(System.Guid orderId)
         {
using (TimeLogger.GetTimeLogger("GetOrderFoodItemsByOrderId"))
{
return await _actor.GetOrderFoodItemsByOrderId(orderId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderItemListResponseDto> GetOrderItemsByOrderId(System.Guid orderId)
         {
using (TimeLogger.GetTimeLogger("GetOrderItemsByOrderId"))
{
return await _actor.GetOrderItemsByOrderId(orderId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.GuidDecimalDictonaryResponseDto> GetOrderItemsCosts(System.Guid orderId)
         {
using (TimeLogger.GetTimeLogger("GetOrderItemsCosts"))
{
return await _actor.GetOrderItemsCosts(orderId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderMakerResponseDto> GetOrderMaker(System.Guid orderId)
         {
using (TimeLogger.GetTimeLogger("GetOrderMaker"))
{
return await _actor.GetOrderMaker(orderId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderReceiptListResponseDto> GetOrderReceiptsByOrderId(System.Guid orderId)
         {
using (TimeLogger.GetTimeLogger("GetOrderReceiptsByOrderId"))
{
return await _actor.GetOrderReceiptsByOrderId(orderId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderListResponseDto> GetOrdersByIds(System.Guid[] ids)
         {
using (TimeLogger.GetTimeLogger("GetOrdersByIds"))
{
return await _actor.GetOrdersByIds(ids);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderStatusListResponseDto> GetOrderStatuses()
         {
using (TimeLogger.GetTimeLogger("GetOrderStatuses"))
{
return await _actor.GetOrderStatuses();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderTypeListResponseDto> GetOrderTypes()
         {
using (TimeLogger.GetTimeLogger("GetOrderTypes"))
{
return await _actor.GetOrderTypes();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredRestaurantOrdersResponseDto> GetPagedActiveOrders(YumaPos.Shared.API.Models.OrderFilterDto filter)
         {
using (TimeLogger.GetTimeLogger("GetPagedActiveOrders"))
{
return await _actor.GetPagedActiveOrders(filter);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredCashDrawerItemResponseDto> GetPagedCashDrawerItems(YumaPos.Shared.API.Models.CashDrawerItemsFilterDto filter)
         {
using (TimeLogger.GetTimeLogger("GetPagedCashDrawerItems"))
{
return await _actor.GetPagedCashDrawerItems(filter);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredRestaurantOrdersResponseDto> GetPagedClosedOrders(YumaPos.Shared.API.Models.OrderFilterDto filter)
         {
using (TimeLogger.GetTimeLogger("GetPagedClosedOrders"))
{
return await _actor.GetPagedClosedOrders(filter);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.EmployeeResponseDto> GetProfile()
         {
using (TimeLogger.GetTimeLogger("GetProfile"))
{
return await _actor.GetProfile();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderReceiptResponseDto> GetReceiptByTransactionId(System.Guid transactionId)
         {
using (TimeLogger.GetTimeLogger("GetReceiptByTransactionId"))
{
return await _actor.GetReceiptByTransactionId(transactionId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.XReportResponseDto> GetReportById(System.Guid cashDrawerItemId)
         {
using (TimeLogger.GetTimeLogger("GetReportById"))
{
return await _actor.GetReportById(cashDrawerItemId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.XReportResponseDto> GetShiftReport()
         {
using (TimeLogger.GetTimeLogger("GetShiftReport"))
{
return await _actor.GetShiftReport();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.LookupListResponseDto> GetSystemSettings(System.String[] listOfSystemSettings)
         {
using (TimeLogger.GetTimeLogger("GetSystemSettings"))
{
return await _actor.GetSystemSettings(listOfSystemSettings);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.TerminalResponseDto> GetTerminal(System.Guid terminalId)
         {
using (TimeLogger.GetTimeLogger("GetTerminal"))
{
return await _actor.GetTerminal(terminalId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.TerminalListResponseDto> GetTerminals()
         {
using (TimeLogger.GetTimeLogger("GetTerminals"))
{
return await _actor.GetTerminals();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.TerminalListResponseDto> GetTerminalsByStoreId(System.Guid storeId)
         {
using (TimeLogger.GetTimeLogger("GetTerminalsByStoreId"))
{
return await _actor.GetTerminalsByStoreId(storeId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerListResponseDto> GetWebCustomers(System.Int32 pageNum, System.Int32 pageSize)
         {
using (TimeLogger.GetTimeLogger("GetWebCustomers"))
{
return await _actor.GetWebCustomers(pageNum, pageSize);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.XReportResponseDto> GetXReport()
         {
using (TimeLogger.GetTimeLogger("GetXReport"))
{
return await _actor.GetXReport();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.BoolResponseDto> IsActiveOrdersUpdated(System.DateTime clientDateTime)
         {
using (TimeLogger.GetTimeLogger("IsActiveOrdersUpdated"))
{
return await _actor.IsActiveOrdersUpdated(clientDateTime);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.BoolResponseDto> IsMenuUpdated(System.DateTime clientUtcDateTime)
         {
using (TimeLogger.GetTimeLogger("IsMenuUpdated"))
{
return await _actor.IsMenuUpdated(clientUtcDateTime);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> Logout()
         {
using (TimeLogger.GetTimeLogger("Logout"))
{
return await _actor.Logout();
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> MarkActiveOrdersAsRead(System.Guid[] ids)
         {
using (TimeLogger.GetTimeLogger("MarkActiveOrdersAsRead"))
{
return await _actor.MarkActiveOrdersAsRead(ids);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> OverrideLogin(System.String password, System.Int32 feature)
         {
using (TimeLogger.GetTimeLogger("OverrideLogin"))
{
return await _actor.OverrideLogin(password, feature);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> PaymentTransaction(YumaPos.Shared.API.Models.RequestTransactionDto requestTransaction)
         {
using (TimeLogger.GetTimeLogger("PaymentTransaction"))
{
return await _actor.PaymentTransaction(requestTransaction);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderFoodResponseDto> RefillGiftCard(YumaPos.Shared.API.Models.GiftCardOrderItemDto item, System.Decimal amount)
         {
using (TimeLogger.GetTimeLogger("RefillGiftCard"))
{
return await _actor.RefillGiftCard(item, amount);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> RemoveAllDiscountsFromOrder(System.Guid orderId)
         {
using (TimeLogger.GetTimeLogger("RemoveAllDiscountsFromOrder"))
{
return await _actor.RemoveAllDiscountsFromOrder(orderId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> RemoveCustomerAddress(System.Guid customerAddressId)
         {
using (TimeLogger.GetTimeLogger("RemoveCustomerAddress"))
{
return await _actor.RemoveCustomerAddress(customerAddressId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> RemoveProductFromOrder(YumaPos.Shared.API.Models.RestaurantOrderItemDto orderItem)
         {
using (TimeLogger.GetTimeLogger("RemoveProductFromOrder"))
{
return await _actor.RemoveProductFromOrder(orderItem);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> RemoveRelatedModifierFromOrderItem(YumaPos.Shared.API.Models.OrderItemRelatedModifierDto relatedModifier)
         {
using (TimeLogger.GetTimeLogger("RemoveRelatedModifierFromOrderItem"))
{
return await _actor.RemoveRelatedModifierFromOrderItem(relatedModifier);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> SavePaymentTransaction(YumaPos.Shared.API.Models.InputTransactionInfoDto data)
         {
using (TimeLogger.GetTimeLogger("SavePaymentTransaction"))
{
return await _actor.SavePaymentTransaction(data);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> SendReceipt(System.Guid orderId, System.String email)
         {
using (TimeLogger.GetTimeLogger("SendReceipt"))
{
return await _actor.SendReceipt(orderId, email);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> TryProcessPaymentTransaction(YumaPos.Shared.API.Models.RequestTransactionDto requestTransaction)
         {
using (TimeLogger.GetTimeLogger("TryProcessPaymentTransaction"))
{
return await _actor.TryProcessPaymentTransaction(requestTransaction);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateCommonModifierQuantityForOrderItem(YumaPos.Shared.API.Models.OrderItemCommonModifierDto modifier)
         {
using (TimeLogger.GetTimeLogger("UpdateCommonModifierQuantityForOrderItem"))
{
return await _actor.UpdateCommonModifierQuantityForOrderItem(modifier);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateCustomer(YumaPos.Shared.API.Models.CustomerDto customer)
         {
using (TimeLogger.GetTimeLogger("UpdateCustomer"))
{
return await _actor.UpdateCustomer(customer);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateOrder(YumaPos.Shared.API.Models.RestaurantOrderDto orderDto)
         {
using (TimeLogger.GetTimeLogger("UpdateOrder"))
{
return await _actor.UpdateOrder(orderDto);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateOrderCustomer(System.Guid orderId, System.Guid customerId)
         {
using (TimeLogger.GetTimeLogger("UpdateOrderCustomer"))
{
return await _actor.UpdateOrderCustomer(orderId, customerId);
}
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateOrderItemQuantity(YumaPos.Shared.API.Models.RestaurantOrderItemDto item)
         {
using (TimeLogger.GetTimeLogger("UpdateOrderItemQuantity"))
{
return await _actor.UpdateOrderItemQuantity(item);
}
         }

	}
}