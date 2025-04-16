using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"));
    
    clientBuilder.AddClient<ServiceBusSender, ServiceBusClientOptions>(
    (_, _, provider) => provider.GetService<ServiceBusClient>()!
        .CreateSender("topic.1")).WithName("topic.1");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/sendhellocommand", async (IAzureClientFactory<ServiceBusSender> senderFactory)  =>
{
    var sender = senderFactory.CreateClient("topic.1");
    var msg = new ServiceBusMessage("Hello"); 
    msg.MessageId = Guid.NewGuid().ToString();

    await sender.SendMessageAsync(msg);
    
    return Results.Ok(msg.MessageId);
})
.WithName("SendHelloCommand")
.WithDescription("Sends a hello command to the queue.");


app.Run();

public partial class Program { }