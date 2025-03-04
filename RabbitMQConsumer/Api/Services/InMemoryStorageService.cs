
namespace Api.Services
{
    public class InMemoryStorageService : IInMemoryStorageService
    {
        private string _item = "Hello world";
        public async Task<string> GetObject()
        {
            await Task.Delay(100);
            return _item;
        }

        public async Task SetObject(string obj)
        {
            await Task.Delay(100);
            _item = obj;
        }
    }
}
