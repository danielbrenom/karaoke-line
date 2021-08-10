namespace KaraokeLine.Interfaces
{
    public interface ICache
    {
        public void AddUpdateCache(string key, object value, System.TimeSpan duration);
        public object GetCache(string key);
        public T GetCache<T>(string key);
        public void DeleteCache(string key);
    }
}