using System;
using System.Reactive.Linq;
using YumaPos.Client.Hardware;
using YumaPos.Hardware.Contracts.Devices;

namespace Y_POS.Core.MockData
{
    public class MockMsrService : IMsrService
    {
        public IObservable<IMsrData> GetDataStream()
        {
            return Observable.Return(new MsrData("123456"));
        }

        private class MsrData : IMsrData
        {
            public bool SuccessfulRead { get; }
            public string ErrorMessage { get; }
            public string Track1Data { get; }
            public string Track2Data { get; }
            public string Track3Data { get; }
            public string AccountNumber { get; }
            public DateTime? ExpDate { get; }
            public string ExpDateOriginal { get; }
            public string Title { get; }
            public string FirstName { get; }
            public string MiddleInitial { get; }
            public string Surname { get; }
            public string Suffix { get; }
            public string ServiceCode { get; }

            public MsrData(string accountNumber)
            {
                AccountNumber = accountNumber;
            }
        }
    }
}
