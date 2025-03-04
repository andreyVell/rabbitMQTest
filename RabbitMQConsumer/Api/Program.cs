
using Api.Consumers;
using Api.Services;
using MassTransit;

namespace Api
{
    public class Program
    {

        //как работают exchanges
        //(default,
        //fanout(всем, бродкаст),
        //direct(конкретной очереди через роутинг кей),
        //topic(по шаблону, по типу регулярных выражений),
        //headers(по роутинг хедерс),
        //deadletter(недоставленные)),
        //queue, как настроить request response, как настроить broadcast
        //ack mode отвечает пропадёт ли сообщение из очереди после прочтения

        //для Publish = fanout exchange
        //для каждого типа сообщения создаётся свой exchange который через биндинги распределяет в очереди 
        //send (создаётся fanout exchange) но с биндингом с очередью
        //GetResponse (создаётся fanout exchange) но с биндингом с очередью
        //RespondAsync (создаётся fanout exchange) но с биндингом с очередью
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IInMemoryStorageService, InMemoryStorageService>();
            builder.Services.AddMassTransit(x =>
            {

                x.AddConsumer<GetRequestConsumer>();
                x.AddConsumer<SetValueConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("admin");
                        h.Password("password");
                    });

                    // Настройка получателя (consumer) для обработки запросов
                    cfg.ReceiveEndpoint("service-queue", e =>
                    {
                        e.ConfigureConsumer<GetRequestConsumer>(context);
                        e.ConfigureConsumer<SetValueConsumer>(context);
                    });
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
