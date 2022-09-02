// See https://aka.ms/new-console-template for more information
using MassTransit;
using MessageContracts;
using GreenPipes;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{

    cfg.Host("localhost");
    cfg.ReceiveEndpoint("payment-service", e =>
    {
        e.UseInMemoryOutbox();
        e.Consumer<InvoiceCreatedConsumer>(c =>
        {
            c.UseMessageRetry(m => m.Interval(5, new TimeSpan(0, 0, 10)));
        });
    });

});


var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await busControl.StartAsync(source.Token);
Console.WriteLine("Payment Microservice now listening!");


try
{
    while (true)
    {
        //ste i while loop listening for messages
        await Task.Delay(100);
    }
}
finally
{
    await busControl.StopAsync();
}


class InvoiceCreatedConsumer : IConsumer<IInvoiceCreated>
{
    public async Task Consume(ConsumeContext<IInvoiceCreated> context) { 
    
    await Task.Run(()=>
        Console.WriteLine($"Received message for invoice number:{context.Message.InvoiceNumber}"));
    }


}