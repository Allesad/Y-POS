﻿using YumaPos.Shared.Core.Reciept.Contracts;
using Y_POS.Properties;

namespace Y_POS.Configuration
{
    internal class ApiConfig : IAPIConfig
    {
        public string ServiceAddress
        {
            get { return Settings.Default.ServiceAddress; }
        }

        public string TerminalId { get { return "3C1E1381-4ABD-4194-83A8-FEAA8C34B406"; } }
        public string Tenant { get { return Settings.Default.Tenant; } }
        public string Token { get { return "36BB81A5-F013-4AAE-90C7-AC90AC685376"; } }
        public string BackOfficeAddress { get { return Settings.Default.BackofficeAddress; } }
        public string AuthorizationAddress { get { return Settings.Default.AuthorizationAddress; } }
    }
}
