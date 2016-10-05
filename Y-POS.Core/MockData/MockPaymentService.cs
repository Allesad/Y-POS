using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using YumaPos.Client.Services;

namespace Y_POS.Core.MockData
{
    public class MockPaymentService : IPaymentService
    {
        public IObservable<IPaymentResponse> ProcessRefund(PaymentParams paymentParams)
        {
            return Observable.Return(new PaymentResponse(true)).Delay(TimeSpan.FromSeconds(1));
        }

        public IObservable<IPaymentResponse> ProcessPayment(PaymentParams paymentParams)
        {
            return Observable.Return(new PaymentResponse(false, "Shit just got serious.")).Delay(TimeSpan.FromSeconds(1));
        }

        public Task<IPaymentResponse> ProcessPaymentAsync(PaymentParams paymentParams)
        {
            return Task.FromResult<IPaymentResponse>(new PaymentResponse(true));
        }

        public Task CancelTransaction()
        {
            throw new NotImplementedException();
        }

        private class PaymentResponse : IPaymentResponse
        {
            public bool IsSuccess { get; private set; }
            public string Message { get; private set; }

            public PaymentResponse(bool isSuccess, string message)
            {
                IsSuccess = isSuccess;
                Message = message;
            }

            public PaymentResponse(bool isSuccess)
            {
                IsSuccess = isSuccess;
                Message = "";
            }
        }
    }
}
