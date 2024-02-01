namespace MasteryTest3.Interfaces
{
    public interface ISessionRepository
    {
        public void SetString(string key, string value);
        public void SetInt(string key, int value);
        public string? GetString(string key);
        public int? GetInt(string key);
    }
}
