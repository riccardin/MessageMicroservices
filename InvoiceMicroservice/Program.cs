// See https://aka.ms/new-console-template for more information
using MassTransit;
using MessageContracts;
using GreenPipes;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{

    cfg.Host("localhost");
    cfg.ReceiveEndpoint("invoice-service", e =>
    {
        e.UseInMemoryOutbox();
        e.Consumer<EventConsumer>(c =>
        {
            c.UseMessageRetry(m => m.Interval(5, new TimeSpan(0, 0, 10)));
        });
    });

});

var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await busControl.StartAsync(source.Token);
Console.WriteLine("Invoice Microservice now listening!");

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
public class EventConsumer : IConsumer<IInvoiceToCreate>
{
    public async Task Consume(ConsumeContext<IInvoiceToCreate> context)
    {
        var newInvoiceNumer = new Random().Next(10000, 99999);
        Console.WriteLine($"Creating Invoice {newInvoiceNumer} for customer: {context.Message.CustomerNumber}");

        context.Message.InvoiceItems.ForEach(i =>
        {
            Console.WriteLine($"With items: Price: {i.Price}, Desc: {i.Description}");
            Console.WriteLine($"Actual distance in Miles: {i.ActualMiliage}, Base rate: {i.BaseRate}");
            Console.WriteLine($"Oversized: {i.IsOversized}, Refrigereted: {i.IsRefrigerated}, Haz Mat: {i.IsHazardousMaterial}");

        });

        await context.Publish<IInvoiceCreated>(new
        {

            InvoiceNumber = newInvoiceNumer,
            InvoiceData = new
            {
                context.Message.CustomerNumber,
                context.Message.InvoiceItems
            }

        });
    }

}