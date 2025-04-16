using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc.Testing;
using simple_worker;

namespace rest_api.tests;

public class AzureServicebusTests(AzureServicebusFixture fixture) : IClassFixture<AzureServicebusFixture>
{
    [Fact]
    public async Task SendHello_AsTheOnlyMessageInTheQueue_ShouldReceiveExactlyOneReply()
    {
        var factory = new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:ServiceBus", fixture.ConnectionString);
        });

        var apiClient = factory.CreateClient();
        var response = await apiClient.PostAsync("/sendhellocommand", new StringContent("{}", Encoding.UTF8, "application/json"));
        var simpleWorker = new SimpleWorker(fixture.ConnectionString, "topic.1", "subscription.3");
        await simpleWorker.ReceiveAndReplyToMessagesAsync();

        var sbHelper = new ServiceBusHelper(fixture.ConnectionString, "topic.1", "subscription.3");
        var messages = await sbHelper.TryConsumeMessagesAsync();
        Assert.Equal(200, (int)response.StatusCode);
        Assert.Single(messages);
        Assert.Equal("World!", Encoding.UTF8.GetString(messages.First().Body));
        Assert.Equal(await response.Content.ReadAsStringAsync(), $"\"{messages.First().CorrelationId}\"");
    }
}

internal class ServiceBusHelper : IAsyncDisposable
{
    private readonly string _cstr;
    private readonly string _topic;
    private readonly string _subscription;
    private ServiceBusReceiver? _receiver;

    public ServiceBusHelper(string connectionString, string topic, string subscription)
    {
        _cstr = connectionString;
        _topic = topic;
        _subscription = subscription;
    }

    public async ValueTask DisposeAsync()
    {
        if (_receiver != null)
        {
            await _receiver.CloseAsync();
            await _receiver.DisposeAsync();
        }
    }

    public async ValueTask<IEnumerable<ServiceBusReceivedMessage>> TryConsumeMessagesAsync()
    {

        await using var client = new ServiceBusClient(_cstr);

        var receiverOptions = new ServiceBusReceiverOptions
        {
            ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete
        };

        _receiver = client.CreateReceiver(_topic, _subscription, receiverOptions);

        var messages = await _receiver.ReceiveMessagesAsync(maxMessages: 100, maxWaitTime: TimeSpan.FromSeconds(1));
        return messages;
    }
}