namespace Wrhs.WebApp.Utils
{
    public interface ICache
    {
        void SetValue(string key, object value);

        object GetValue(string key);
    }
}