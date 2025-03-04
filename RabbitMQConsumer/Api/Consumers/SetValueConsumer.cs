using Api.Services;
using Bus.Contract.Models;
using MassTransit;

namespace Api.Consumers
{
    public class SetValueConsumer : IConsumer<SetValueRequest>
    {
        private readonly IInMemoryStorageService _service;
        public SetValueConsumer(IInMemoryStorageService service)
        {
            _service = service;
        }
        public async Task Consume(ConsumeContext<SetValueRequest> context)
        {
            var request = context.Message;

            // Пример обработки запроса
            var result = $"Processed: {request.Value}";

            Console.WriteLine(result);
            // Возвращаем ответ
            await _service.SetObject(request.Value ?? "");
        }
    }
}
