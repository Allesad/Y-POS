using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YumaPos.Client.LocalData.Repositories;
using YumaPos.Client.LocalData.Tables.Settings;

namespace Y_POS.Core.MockData
{
    public class MockCommonClientSettingsRepository : ICommonClientSettingsRepository
    {


        public Task<List<CommonSettings>> GetAllSettingsAsync()
        {
            return Task.FromResult(new List<CommonSettings>
            {
                new CommonSettings {Key = SettingKeys.TERMINAL_ID, Value = "45497989"},
                new CommonSettings {Key = SettingKeys.TERMINAL_NAME, Value = "Allesad"},
                new CommonSettings {Key = SettingKeys.TERMINAL_TOKEN, Value = "36BB81A5-F013-4AAE-90C7-AC90AC685376"},
                new CommonSettings {Key = SettingKeys.TENTANT_NAME, Value = "backoffice.demo.yumapos.com"},
                new CommonSettings {Key = SettingKeys.STORE_ID, Value = "eb0a61d6-1e50-4799-89b1-02346ae6a5c2"}
            });
        }

        public Task<CommonSettings> GetSettingsByKeyAsync(string key)
        {
            throw new NotImplementedException();
        }

        public Task SetValueByKeyAsync(string key, string value)
        {
            throw new NotImplementedException();
        }

        public Task SetValuesAsync(IEnumerable<KeyValuePair<string, string>> pairs)
        {
            throw new NotImplementedException();
        }
    }
}
