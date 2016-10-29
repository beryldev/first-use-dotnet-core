namespace Wrhs.WebApp
{
    public interface ICache
    {
        void SetValue(string key, object value);

        object GetValue(string key);
    }
}