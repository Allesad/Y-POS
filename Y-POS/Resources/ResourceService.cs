using System.Resources;
using YumaPos.Client.Common;

namespace Y_POS.Resources
{
    internal class ResourceService : IResourcesService
    {
        #region Fields

        private readonly ResourceManager _rm;

        #endregion

        #region Constructor

        public ResourceService()
        {
            _rm = Core.Properties.Resources.ResourceManager;
        }

        #endregion

        #region IResourcesService

        public T GetResource<T>(string resourceKey)
        {
            return (T) _rm.GetObject(resourceKey);
        }

        public bool TryGetResource<T>(string resourceKey, out T output) where T : class
        {
            output = _rm.GetString(resourceKey) as T;
            return output != null;
        }

        public string GetNameOfEnum<T>(T enumValue) where T : struct
        {
            return _rm.GetString($"{typeof (T).Name}_{enumValue}");
        }

        public string GetStub(string key)
        {
            return key;
        }

        #endregion
    }
}
