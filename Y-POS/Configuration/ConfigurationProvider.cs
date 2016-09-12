using System;
using YumaPos.Client.Configuration;
using Y_POS.Properties;

namespace Y_POS.Configuration
{
    internal class ConfigurationProvider : IConfigurationProvider
    {
        public string ServiceAddress
        {
            get { return Settings.Default.ServiceAddress; }
        }

        public string BackOfficeAddress { get; }
        public string AuthorizationAddress { get; }
        public string LocalDbName { get; }
        public string AppName { get; }
        public Uri DefaultImageUri { get; }
        public string ReportsAddress { get; }
        public string ImageServiceAddress { get; }
        public string LocalDirectoryPath { get; }
    }
}
