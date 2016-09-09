using System;
using System.Threading.Tasks;
using YumaPos.Client.Account;
using YumaPos.Shared.API;

namespace Y_POS.Core.MockData
{
    public class MockAccountServiceManager : IAccountServiceManager
    {
        #region Fields

        private readonly IAuthorizationApi _authorizationApi;

        #endregion

        #region Constructor

        public MockAccountServiceManager(IAuthorizationApi authorizationApi)
        {
            if (authorizationApi == null) throw new ArgumentNullException(nameof(authorizationApi));

            _authorizationApi = authorizationApi;
        }

        #endregion

        public ILoggedUser User { get; private set; }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var token = await _authorizationApi.Login(username, password).ConfigureAwait(false);

            User = new LoggedUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Jack",
                LastName = "Smartass",
                Token = token,
                IsCanOpenCashDrawer = true
            };

            return true;
        }

        public Task<bool> LoginByPin(string pin)
        {
            return LoginAsync(string.Empty, pin);
        }

        public Task<bool> LoginToBackoffice(string login, string password)
        {
            throw new NotImplementedException();
        }

        public Task Logout()
        {
            User = null;
            return Task.FromResult(0);
        }

        private class LoggedUser : ILoggedUser
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get { return string.Join(" ", FirstName, LastName); } }
            public Guid? ImageId { get; set; }
            public string Token { get; set; }
            public bool HasBackofficeAccess { get; set; }
            public bool IsCanOpenCashDrawer { get; set; }
        }
    }
}
