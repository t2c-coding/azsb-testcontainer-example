using System.Diagnostics;
using Testcontainers.ServiceBus;

public class AzureServicebusFixture : IAsyncLifetime
{
    private readonly ServiceBusContainer _serviceBusContainer;

    public string ConnectionString => _serviceBusContainer.GetConnectionString();

    public AzureServicebusFixture()
    {
        _serviceBusContainer = new ServiceBusBuilder()
            .WithImage("mcr.microsoft.com/azure-messaging/servicebus-emulator:latest")
            .WithAcceptLicenseAgreement(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        Debug.WriteLine("Starting Azure Service Bus emulator...");
        await _serviceBusContainer.StartAsync();
        Debug.WriteLine("Azure Service Bus emulator started.");
    }
    
    public async Task DisposeAsync()
    {
        try
        {
            Debug.WriteLine("Stopping Azure Service Bus emulator...");
            await _serviceBusContainer.StopAsync();
            Debug.WriteLine("Azure Service Bus emulator stopped.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error stopping Azure Service Bus emulator: {ex.Message}");
        }
    }
}