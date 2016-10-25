
using System;
using System.Linq;
using System.ServiceModel;
using YumaPos.Shared.Infrastructure;
using YumaPos.Client.Navigation.Contracts;
using YumaPos.FrontEnd.Infrastructure.CommandProcessing;
using Y_POS.Core.Infrastructure.Exceptions;

namespace Y_POS.Core.Infrastructure.Decorators
{
	public sealed class UnauthorizeErrorApiDecorator : ITerminalApi
	{
        #region Fields

        private readonly ITerminalApi _actor;

        #endregion

		#region Properties

		public ExecutionContext ExecutionContext 
        {
            get { return _actor.ExecutionContext; }
            set { _actor.ExecutionContext = value; }
        }

		#endregion

		public UnauthorizeErrorApiDecorator(ITerminalApi actor)
		{
			if (actor == null) throw new ArgumentNullException(nameof(actor));

		    _actor = actor;
		}

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateOrderNote(System.Guid orderId, System.String note)
         {
           try
            {
                return await _actor.UpdateOrderNote(orderId, note);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateOrderStatusByOrderId(System.Guid orderId, System.Int32 statusId)
         {
           try
            {
                return await _actor.UpdateOrderStatusByOrderId(orderId, statusId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateProfile(YumaPos.Shared.API.Models.EmployeeDto model)
         {
           try
            {
                return await _actor.UpdateProfile(model);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdatePushToken(YumaPos.Shared.API.Enums.PushType pushType, System.String pushToken)
         {
           try
            {
                return await _actor.UpdatePushToken(pushType, pushToken);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateRelatedModifierQuantityForOrderItem(System.Guid orderId, YumaPos.Shared.API.Models.OrderItemRelatedModifierDto modifier)
         {
           try
            {
                return await _actor.UpdateRelatedModifierQuantityForOrderItem(orderId, modifier);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.BoolResponseDto> UpdateSplittingsForOrderId(System.Guid orderId, YumaPos.Shared.API.Enums.SplittingType splittingType, System.String[] parameters)
         {
           try
            {
                return await _actor.UpdateSplittingsForOrderId(orderId, splittingType, parameters);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateTaxExempt(System.Guid orderId, System.Int32 splittingNumber, System.Boolean isTaxExempt)
         {
           try
            {
                return await _actor.UpdateTaxExempt(orderId, splittingNumber, isTaxExempt);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateTerminal(YumaPos.Shared.API.Models.TerminalDto terminal)
         {
           try
            {
                return await _actor.UpdateTerminal(terminal);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async void SetUserToken(System.String token)
         {
            _actor.SetUserToken(token);
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.MenuCacheResponseDto> GetTerminalCacheMenu()
         {
           try
            {
                return await _actor.GetTerminalCacheMenu();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> AddCashDrawerCheckItem(YumaPos.Shared.API.Models.CashDrawerItemDto itemDto)
         {
           try
            {
                return await _actor.AddCashDrawerCheckItem(itemDto);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.GuidResponseDto> AddCustomer(YumaPos.Shared.API.Models.CustomerDto customerDto)
         {
           try
            {
                return await _actor.AddCustomer(customerDto);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.IntResponseDto> AddCustomersGroup(YumaPos.Shared.API.Models.CustomerGroupDto customerGroup)
         {
           try
            {
                return await _actor.AddCustomersGroup(customerGroup);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> AddDiscountToOrder(System.Guid orderId, System.Guid discountId)
         {
           try
            {
                return await _actor.AddDiscountToOrder(orderId, discountId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> AddDiscountToSplitting(System.Guid orderId, System.Int32 splittingNumber, Guid? discountId)
         {
           try
            {
                return await _actor.AddDiscountToSplitting(orderId, splittingNumber, discountId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderFoodResponseDto> AddGiftCardToOrder(YumaPos.Shared.API.Models.GiftCardOrderItemDto item)
         {
           try
            {
                return await _actor.AddGiftCardToOrder(item);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderResponseDto> AddOrder(System.Guid orderId, YumaPos.Shared.API.Enums.OrderType orderType)
         {
           try
            {
                return await _actor.AddOrder(orderId, orderType);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderFoodResponseDto> AddOrderItem(YumaPos.Shared.API.Models.RestaurantOrderItemDto item)
         {
           try
            {
                return await _actor.AddOrderItem(item);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> AddRelatedModifierToOrderItem(System.Guid orderId, YumaPos.Shared.API.Models.OrderItemRelatedModifierDto relatedModifier)
         {
           try
            {
                return await _actor.AddRelatedModifierToOrderItem(orderId, relatedModifier);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.GuidResponseDto> AddTerminalToStore(YumaPos.Shared.API.Models.TerminalDto terminalDto)
         {
           try
            {
                return await _actor.AddTerminalToStore(terminalDto);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> CashDrawerMakeLoan(YumaPos.Shared.API.Models.PickUpDto pickUpDto)
         {
           try
            {
                return await _actor.CashDrawerMakeLoan(pickUpDto);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> CashDrawerMakePickUp(YumaPos.Shared.API.Models.PickUpDto pickUpDto)
         {
           try
            {
                return await _actor.CashDrawerMakePickUp(pickUpDto);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> ChangePassword(YumaPos.Shared.API.Models.ChangePasswordDto model)
         {
           try
            {
                return await _actor.ChangePassword(model);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.BoolResponseDto> CheckEmployeePin(System.Int32 pin)
         {
           try
            {
                return await _actor.CheckEmployeePin(pin);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.DecimalResponseDto> CheckGiftCardBalance(System.String cardNumber)
         {
           try
            {
                return await _actor.CheckGiftCardBalance(cardNumber);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> CheckIn(System.String cardNo)
         {
           try
            {
                return await _actor.CheckIn(cardNo);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> CheckOut(System.String cardNo)
         {
           try
            {
                return await _actor.CheckOut(cardNo);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> ClockOperation(YumaPos.Shared.API.Enums.UserActivityType operationType)
         {
           try
            {
                return await _actor.ClockOperation(operationType);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> CloseOrder(System.Guid orderId)
         {
           try
            {
                return await _actor.CloseOrder(orderId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderListResponseDto> GetActiveOrders()
         {
           try
            {
                return await _actor.GetActiveOrders();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ActiveOrdersChangesDigestResponseDto> GetActiveOrdersChangesDigest(System.DateTime timestamp)
         {
           try
            {
                return await _actor.GetActiveOrdersChangesDigest(timestamp);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredRestaurantOrdersResponseDto> GetActiveOrdersRange(YumaPos.Shared.API.Models.ActiveOrdersFilterDto filter)
         {
           try
            {
                return await _actor.GetActiveOrdersRange(filter);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerGroupListResponseDto> GetAllCustomersGroups()
         {
           try
            {
                return await _actor.GetAllCustomersGroups();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.DiscountListResponseDto> GetAllDiscounts()
         {
           try
            {
                return await _actor.GetAllDiscounts();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.GiftCardTypeListResponseDto> GetAllGiftCardTypes()
         {
           try
            {
                return await _actor.GetAllGiftCardTypes();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.StoreListResponseDto> GetAllStores()
         {
           try
            {
                return await _actor.GetAllStores();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CashDrawerInfoResponseDto> GetCashDrawerInfoTotal()
         {
           try
            {
                return await _actor.GetCashDrawerInfoTotal();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CashDrawerItemListResponseDto> GetCashDrawerItems(YumaPos.Shared.API.Models.CashDrawerItemsFilterDto filter)
         {
           try
            {
                return await _actor.GetCashDrawerItems(filter);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CheckoutOptionListResponseDto> GetCheckoutOptionsByOrderType(YumaPos.Shared.API.Enums.OrderType orderType)
         {
           try
            {
                return await _actor.GetCheckoutOptionsByOrderType(orderType);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CashDrawerItemResponseDto> GetCurrentCashierLastActivity()
         {
           try
            {
                return await _actor.GetCurrentCashierLastActivity();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.EmployeeResponseDto> GetCurrentEmployee()
         {
           try
            {
                return await _actor.GetCurrentEmployee();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.StoreResponseDto> GetCurrentStore()
         {
           try
            {
                return await _actor.GetCurrentStore();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.UserClockStateResponseDto> GetCurrentUserClockState()
         {
           try
            {
                return await _actor.GetCurrentUserClockState();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerResponseDto> GetCustomer(System.Guid customerId)
         {
           try
            {
                return await _actor.GetCustomer(customerId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerAddressListResponseDto> GetCustomerAddresses(System.Guid customerId)
         {
           try
            {
                return await _actor.GetCustomerAddresses(customerId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerListResponseDto> GetCustomersByGroup(System.Int32 groupId, System.Int32 pageNum, System.Int32 pageSize)
         {
           try
            {
                return await _actor.GetCustomersByGroup(groupId, pageNum, pageSize);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerListResponseDto> GetCustomersFiltered(YumaPos.Shared.API.Models.CustomerFilterDto filters)
         {
           try
            {
                return await _actor.GetCustomersFiltered(filters);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<System.Byte[]> GetDefaultImage(YumaPos.Shared.API.Enums.ImageSizeType imageSizeType)
         {
           try
            {
                return await _actor.GetDefaultImage(imageSizeType);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredCustomersResponseResponseDto> GetFilteredCustomers(YumaPos.Shared.API.Models.FilteredRequestDto model)
         {
           try
            {
                return await _actor.GetFilteredCustomers(model);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredCustomersOrdersResponseResponseDto> GetFilteredCustomersOrders(YumaPos.Shared.API.Models.FilteredRequestDto model)
         {
           try
            {
                return await _actor.GetFilteredCustomersOrders(model);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomersSummaryResponseDto> GetFilteredCustomersSummary(YumaPos.Shared.API.Models.FilteredRequestFilterDto[] model)
         {
           try
            {
                return await _actor.GetFilteredCustomersSummary(model);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderListResponseDto> GetFilteredOrdersByIds(YumaPos.Shared.API.Models.OrderFilterDto filter)
         {
           try
            {
                return await _actor.GetFilteredOrdersByIds(filter);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerGroupListResponseDto> GetGroupsWithCustomerAmount()
         {
           try
            {
                return await _actor.GetGroupsWithCustomerAmount();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CashierShiftResponseDto> GetLastCashierShift()
         {
           try
            {
                return await _actor.GetLastCashierShift();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderResponseDto> GetOrderById(System.Guid orderId)
         {
           try
            {
                return await _actor.GetOrderById(orderId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderFoodListResponseDto> GetOrderFoodItemsByOrderId(System.Guid orderId)
         {
           try
            {
                return await _actor.GetOrderFoodItemsByOrderId(orderId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderItemListResponseDto> GetOrderItemsByOrderId(System.Guid orderId)
         {
           try
            {
                return await _actor.GetOrderItemsByOrderId(orderId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.GuidDecimalDictonaryResponseDto> GetOrderItemsCosts(System.Guid orderId)
         {
           try
            {
                return await _actor.GetOrderItemsCosts(orderId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderMakerResponseDto> GetOrderMaker(System.Guid orderId)
         {
           try
            {
                return await _actor.GetOrderMaker(orderId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderReceiptListResponseDto> GetOrderReceiptsByOrderId(System.Guid orderId)
         {
           try
            {
                return await _actor.GetOrderReceiptsByOrderId(orderId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderListResponseDto> GetOrdersByIds(System.Guid[] ids)
         {
           try
            {
                return await _actor.GetOrdersByIds(ids);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderStatusListResponseDto> GetOrderStatuses()
         {
           try
            {
                return await _actor.GetOrderStatuses();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderTypeListResponseDto> GetOrderTypes()
         {
           try
            {
                return await _actor.GetOrderTypes();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredRestaurantOrdersResponseDto> GetPagedActiveOrders(YumaPos.Shared.API.Models.OrderFilterDto filter)
         {
           try
            {
                return await _actor.GetPagedActiveOrders(filter);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredCashDrawerItemResponseDto> GetPagedCashDrawerItems(YumaPos.Shared.API.Models.CashDrawerItemsFilterDto filter)
         {
           try
            {
                return await _actor.GetPagedCashDrawerItems(filter);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.FilteredRestaurantOrdersResponseDto> GetPagedClosedOrders(YumaPos.Shared.API.Models.OrderFilterDto filter)
         {
           try
            {
                return await _actor.GetPagedClosedOrders(filter);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.EmployeeResponseDto> GetProfile()
         {
           try
            {
                return await _actor.GetProfile();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.RestaurantOrderReceiptResponseDto> GetReceiptByTransactionId(System.Guid transactionId)
         {
           try
            {
                return await _actor.GetReceiptByTransactionId(transactionId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.XReportResponseDto> GetReportById(System.Guid cashDrawerItemId)
         {
           try
            {
                return await _actor.GetReportById(cashDrawerItemId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.XReportResponseDto> GetShiftReport()
         {
           try
            {
                return await _actor.GetShiftReport();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.LookupListResponseDto> GetSystemSettings(System.String[] listOfSystemSettings)
         {
           try
            {
                return await _actor.GetSystemSettings(listOfSystemSettings);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.TerminalResponseDto> GetTerminal(System.Guid terminalId)
         {
           try
            {
                return await _actor.GetTerminal(terminalId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.TerminalListResponseDto> GetTerminals()
         {
           try
            {
                return await _actor.GetTerminals();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.TerminalListResponseDto> GetTerminalsByStoreId(System.Guid storeId)
         {
           try
            {
                return await _actor.GetTerminalsByStoreId(storeId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.CustomerListResponseDto> GetWebCustomers(System.Int32 pageNum, System.Int32 pageSize)
         {
           try
            {
                return await _actor.GetWebCustomers(pageNum, pageSize);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.XReportResponseDto> GetXReport()
         {
           try
            {
                return await _actor.GetXReport();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.BoolResponseDto> IsActiveOrdersUpdated(System.DateTime clientDateTime)
         {
           try
            {
                return await _actor.IsActiveOrdersUpdated(clientDateTime);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.BoolResponseDto> IsMenuUpdated(System.DateTime clientUtcDateTime)
         {
           try
            {
                return await _actor.IsMenuUpdated(clientUtcDateTime);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> Logout()
         {
           try
            {
                return await _actor.Logout();
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> MarkActiveOrdersAsRead(System.Guid[] ids)
         {
           try
            {
                return await _actor.MarkActiveOrdersAsRead(ids);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> OverrideLogin(System.String password, System.Int32 feature)
         {
           try
            {
                return await _actor.OverrideLogin(password, feature);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> PaymentTransaction(YumaPos.Shared.API.Models.RequestTransactionDto requestTransaction)
         {
           try
            {
                return await _actor.PaymentTransaction(requestTransaction);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.OrderFoodResponseDto> RefillGiftCard(YumaPos.Shared.API.Models.GiftCardOrderItemDto item, System.Decimal amount)
         {
           try
            {
                return await _actor.RefillGiftCard(item, amount);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> RemoveAllDiscountsFromOrder(System.Guid orderId)
         {
           try
            {
                return await _actor.RemoveAllDiscountsFromOrder(orderId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> RemoveCustomerAddress(System.Guid customerAddressId)
         {
           try
            {
                return await _actor.RemoveCustomerAddress(customerAddressId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> RemoveProductFromOrder(YumaPos.Shared.API.Models.RestaurantOrderItemDto orderItem)
         {
           try
            {
                return await _actor.RemoveProductFromOrder(orderItem);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> RemoveRelatedModifierFromOrderItem(YumaPos.Shared.API.Models.OrderItemRelatedModifierDto relatedModifier)
         {
           try
            {
                return await _actor.RemoveRelatedModifierFromOrderItem(relatedModifier);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> SavePaymentTransaction(YumaPos.Shared.API.Models.InputTransactionInfoDto data)
         {
           try
            {
                return await _actor.SavePaymentTransaction(data);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> SendReceipt(System.Guid orderId, System.String email)
         {
           try
            {
                return await _actor.SendReceipt(orderId, email);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> TryProcessPaymentTransaction(YumaPos.Shared.API.Models.RequestTransactionDto requestTransaction)
         {
           try
            {
                return await _actor.TryProcessPaymentTransaction(requestTransaction);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateCommonModifierQuantityForOrderItem(YumaPos.Shared.API.Models.OrderItemCommonModifierDto modifier)
         {
           try
            {
                return await _actor.UpdateCommonModifierQuantityForOrderItem(modifier);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateCustomer(YumaPos.Shared.API.Models.CustomerDto customer)
         {
           try
            {
                return await _actor.UpdateCustomer(customer);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateOrder(YumaPos.Shared.API.Models.RestaurantOrderDto orderDto)
         {
           try
            {
                return await _actor.UpdateOrder(orderDto);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateOrderCustomer(System.Guid orderId, System.Guid customerId)
         {
           try
            {
                return await _actor.UpdateOrderCustomer(orderId, customerId);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

         public async System.Threading.Tasks.Task<YumaPos.Shared.API.ResponseDtos.ResponseDto> UpdateOrderItemQuantity(YumaPos.Shared.API.Models.RestaurantOrderItemDto item)
         {
           try
            {
                return await _actor.UpdateOrderItemQuantity(item);
            }
            catch (AggregateException ex)
            {
                if (ex.Flatten().InnerExceptions.OfType<FaultException>().Any(e => e.Code.Name.Equals("401"))) throw new UnauthorizedException();
                throw;
            }
         }

	}
}