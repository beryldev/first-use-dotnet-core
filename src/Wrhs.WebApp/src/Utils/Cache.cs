using Microsoft.Extensions.Caching.Memory;

namespace Wrhs.WebApp.Utils
{
    public class Cache : ICache
    {
        IMemoryCache memCache;

        public Cache(IMemoryCache memCache)
        {
            this.memCache = memCache;
        }
        
        public object GetValue(string key)
        {
            return memCache.Get(key);
        }

        public void SetValue(string key, object value)
        {
            memCache.Set(key, value);
        }
    }
}