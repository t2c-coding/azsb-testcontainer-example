using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddServiceBusClient(builder.Configuration.GetConnectionString("ConnectionStrings:ServiceBus"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/sendhellocommand", static async (IAzureClientFactory<ServiceBusSender> senderFactory)  =>
{
    var sender = senderFactory.CreateClient("HelloCommandQueue");
    await sender.SendMessageAsync(new ServiceBusMessage("Hello World!"));
    return Results.Ok();
})
.WithName("SendHelloCommand")
.WithDescription("Sends a hello command to the queue.");


app.Run();
