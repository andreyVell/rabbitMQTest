namespace Api.Services
{
    public interface IBusService
    {
        public Task<string> GetAsync();
        public Task SetAsync(string obj);
    }
}
