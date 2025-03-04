
using Bus.Contract.Models;
using MassTransit;

namespace Api.Services
{
    public class BusService : IBusService
    {
        private readonly IBus _bus;
        public BusService(IBus bus)
        {
            _bus = bus;
        }
        public async Task<string> GetAsync()
        {
            var requestClient = _bus.CreateRequestClient<GetValueRequest>();
            var response = await requestClient.GetResponse<GetValueResponse>(new GetValueRequest());
            return response.Message.Result ?? "";
        }

        public async Task SetAsync(string obj)
        {
            //direct отправка
            var endpoint = await _bus.GetSendEndpoint(new Uri("queue:service-queue"));
            await endpoint.Send(new SetValueRequest { Value = obj });
        }
    }
}
