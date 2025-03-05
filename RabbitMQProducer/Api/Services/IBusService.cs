namespace Api.Services
{
    public interface IBusService
    {
        public Task<string> GetResponseAsync();
        public Task SendAsync(string obj);
    }
}
