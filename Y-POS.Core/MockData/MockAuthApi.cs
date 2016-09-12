using System;
using System.Threading.Tasks;
using YumaPos.Shared.API;

namespace Y_POS.Core.MockData
{
    public class MockAuthApi : IAuthorizationApi
    {
        public async Task<string> Login(string login, string password)
        {
            await Task.Delay(1000);
            return Guid.NewGuid().ToString();
        }
    }
}
