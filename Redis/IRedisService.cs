namespace ApplicationTracker.Redis
{
    public interface IRedisService
    {
        T? GetData<T>(string key);
        void SetData<T>(string key, T value);
    }
}
