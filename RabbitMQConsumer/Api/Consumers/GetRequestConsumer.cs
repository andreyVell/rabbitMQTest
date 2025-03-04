using Api.Services;
using Bus.Contract.Models;
using MassTransit;

namespace Api.Consumers
{
    public class GetRequestConsumer : IConsumer<GetValueRequest>
    {
        private readonly IInMemoryStorageService _service;
        public GetRequestConsumer(IInMemoryStorageService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<GetValueRequest> context)
        {
            var request = context.Message;

            // Пример обработки запроса
            var result = $"Processed: something";

            Console.WriteLine(result);
            
            // Возвращаем ответ
            await context.RespondAsync(new GetValueResponse { Result = await _service.GetObject() });
        }
    }
}
