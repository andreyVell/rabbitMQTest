
using Api.Consumers;
using Api.Services;
using MassTransit;

namespace Api
{
    public class Program
    {

        //��� �������� exchanges
        //(default,
        //fanout(����, ��������),
        //direct(���������� ������� ����� ������� ���),
        //topic(�� �������, �� ���� ���������� ���������),
        //headers(�� ������� ������),
        //deadletter(��������������)),
        //queue, ��� ��������� request response, ��� ��������� broadcast
        //ack mode �������� ������� �� ��������� �� ������� ����� ���������

        //��� Publish = fanout exchange
        //��� ������� ���� ��������� �������� ���� exchange ������� ����� �������� ������������ � ������� 
        //send (�������� fanout exchange) �� � ��������� � ��������
        //GetResponse (�������� fanout exchange) �� � ��������� � ��������
        //RespondAsync (�������� fanout exchange) �� � ��������� � ��������
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

                    // ��������� ���������� (consumer) ��� ��������� ��������
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
