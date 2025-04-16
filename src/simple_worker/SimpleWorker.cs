using System.Diagnostics;
using System.Text;
using Azure.Messaging.ServiceBus;

namespace simple_worker;

public class SimpleWorker : IAsyncDisposable
{
    private readonly string _cstring;
    private readonly string _topic;
    private readonly string _subscription;
    private ServiceBusReceiver? _receiver;
    private ServiceBusSender? _sender;

    public SimpleWorker(string servicebusConnectionString, string topic, string subscription)
    {
        _cstring = servicebusConnectionString;
        _topic = topic;
        _subscription = subscription;
    }

    public async ValueTask DisposeAsync()
    {
        if(_receiver != null) await _receiver.DisposeAsync();
        if(_sender != null) await _sender.DisposeAsync();

        Debug.WriteLine("SimpleWorker disposed");

    }

    public async Task ReceiveAndReplyToMessagesAsync(CancellationToken stoppingToken = default)
    {
        await using var client = new ServiceBusClient(_cstring);
        var _receiverOptions = new ServiceBusReceiverOptions
        {
            ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete
        };

        _receiver = client.CreateReceiver(_topic, _subscription, _receiverOptions);
        _sender = client.CreateSender(_topic);

        try
        {
            var messages = await _receiver.ReceiveMessagesAsync(maxMessages: 100, maxWaitTime: TimeSpan.FromSeconds(1));
            foreach (var message in messages)
            {
                if (message.Body != null)
                {
                    var body = Encoding.UTF8.GetString(message.Body.ToArray());
                    Debug.WriteLine($"Received message: {body}");

                    var instruction = body.Substring(4, body.Length - 4);
                    var msg = new ServiceBusMessage(instruction);
                    msg.CorrelationId = message.MessageId;
                    await _sender.SendMessageAsync(msg);
                    Debug.WriteLine("Sent reply message back!");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error receiving/sending messages: {ex.Message}");
        }
        finally
        {
            await _receiver.CloseAsync();

            await _sender.CloseAsync();
            Debug.WriteLine("_receiver closed.");
        }

    }
}
