using System;
using System.Threading.Tasks;
using YumaPos.Client.LocalData.Repositories;
using YumaPos.Client.LocalData.Tables.Settings;

namespace Y_POS.Core.MockData
{
    public class MockStoreSettingsRepository : IStoreSettingsRepository
    {
        public Task<StoreSettings> GetStoreSettingsAsync(string storeId)
        {
            return Task.FromResult(new StoreSettings
            {
                StoreId = "eb0a61d6-1e50-4799-89b1-02346ae6a5c2",
                Title = "Test - Default Store",
                Country = "1049",
                City = "St. Peterburg",
                Street = "7th line V.O.",
                ZipCode = "199048",
                Latitude = 19.10455m,
                Longitude = 72.83382m
            });
        }

        public Task UpdateStoreSettings(string storeId, StoreSettings value)
        {
            throw new NotImplementedException();
        }
    }
}


