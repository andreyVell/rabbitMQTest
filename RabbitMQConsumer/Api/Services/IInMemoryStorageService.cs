namespace Api.Services
{
    public interface IInMemoryStorageService
    {
        public Task<string> GetObject();
        public Task SetObject(string obj);
    }
}
