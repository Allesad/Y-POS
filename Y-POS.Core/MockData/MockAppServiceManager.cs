using System;
using System.Threading.Tasks;
using YumaPos.Client.App;

namespace Y_POS.Core.MockData
{
    public class MockAppServiceManager : IAppServiceManager
    {
        #region Fields

        private readonly TerminalData _terminal = new TerminalData();
        private readonly StoreData _store = new StoreData();

        #endregion

        public ITerminal Terminal
        {
            get { return _terminal; }
        }

        public IStore Store
        {
            get { return _store; }
        }

        public bool IsTerminalRegistered
        {
            get { return !string.IsNullOrEmpty(_terminal.Id) && !string.IsNullOrEmpty(_store.Id); }
        }


        public Task InitAsync()
        {
            return PrepareTerminalAsync(_terminal)
                .ContinueWith(task => PrepareStoreAsync(_store), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public void SetTempTenant(string tenant)
        {
            
        }

        public Task SetTenantAsync(string tenant)
        {
            if (string.IsNullOrEmpty(tenant))
                throw new ArgumentException("Tenant cannot be null or empty!", "tenant");

            _terminal.Tenant = tenant;

            return Task.FromResult(0);
        }

        public Task SetTerminalInfoAsync(string id, string name, string token)
        {
            _terminal.Id = id;
            _terminal.Name = name;
            _terminal.Token = token;

            return Task.FromResult(0);
        }

        public Task SetStoreAsync(IStore store)
        {
            _terminal.StoreId = store.Id;
            _store.Id = store.Id;
            _store.Title = store.Title;
            _store.Logo = store.Logo;
            _store.Phone = store.Phone;
            _store.Country = store.Country;
            _store.State = store.State;
            _store.City = store.City;
            _store.Street = store.Street;
            _store.Building = store.Building;
            _store.ZipCode = store.ZipCode;
            _store.Latitude = store.Latitude;
            _store.Longitude = store.Longitude;

            return Task.FromResult(0);
        }

        #region Private methods

        private static readonly string TempStoreId = Guid.NewGuid().ToString();

        private async Task PrepareTerminalAsync(TerminalData terminal)
        {
            terminal.Id = Guid.NewGuid().ToString();
            terminal.Name = "Watson Terminal";
            terminal.Token = Guid.NewGuid().ToString();
            terminal.Tenant = "demo.yumapos.com";
            terminal.StoreId = TempStoreId;
        }

        private async Task PrepareStoreAsync(StoreData store)
        {
            if (!_terminal.HasStore)
                return;

            store.Id = TempStoreId;
            store.Title = "Drugs 13";
            store.Logo = string.Empty;
            store.Phone = "4857896372";
            store.Country = "Motherland";
            store.State = string.Empty;
            store.City = "City 17";
            store.Street = "6 line, VO";
            store.Building = "59";
            store.ZipCode = "392835";
            store.Latitude = 59.948817m;
            store.Longitude = 30.272210m;
        }

        #endregion

        private class TerminalData : ITerminal
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Token { get; set; }
            public string Tenant { get; set; }
            public string StoreId { get; set; }

            public bool HasTenant
            {
                get { return !string.IsNullOrEmpty(Tenant); }
            }

            public bool HasToken
            {
                get { return !string.IsNullOrEmpty(Token); }
            }

            public bool HasStore
            {
                get { return !string.IsNullOrEmpty(StoreId); }
            }
        }

        private class StoreData : IStore
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Logo { get; set; }
            public string Phone { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            public string Building { get; set; }
            public string ZipCode { get; set; }
            public decimal? Latitude { get; set; }
            public decimal? Longitude { get; set; }
        }
    }
}