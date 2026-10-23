using Microsoft.Extensions.Configuration;
using MQTTnet;
using System.Text.Json;

public class MqttConsumerService
{
    private readonly TelemetryDisplayService _displayService;

    public MqttConsumerService(
        TelemetryDisplayService displayService)
    {
        _displayService = displayService;
    }

    public async Task StartAsync()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        string host = config["Mqtt:Host"] ?? "localhost";
        int port = int.Parse(config["Mqtt:Port"] ?? "1883");
        string username = config["Mqtt:Username"] ?? "";
        string password = config["Mqtt:Password"] ?? "";
        string topic = config["Mqtt:Topic"] ?? "sensor/#";

        var factory = new MqttClientFactory();
        using var mqtt = factory.CreateMqttClient();

        mqtt.ApplicationMessageReceivedAsync += e =>
        {
            string payload =
                e.ApplicationMessage.ConvertPayloadToString();

            try
            {
                var message =
                    JsonSerializer.Deserialize<TelemetryMessage>(
                        payload,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (message is not null)
                {
                    message.Timestamp = DateTimeOffset.Now;
                    _displayService.Display(message);
                }
            }
            catch (JsonException)
            {
                Console.WriteLine(
                    $"Kunde inte läsa MQTT-meddelandet: {payload}");
            }

            return Task.CompletedTask;
        };

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(host, port)
            .WithCredentials(username, password)
            .Build();

        await mqtt.ConnectAsync(options);
        await mqtt.SubscribeAsync(topic);

        Console.WriteLine("Ansluten till MQTT.");
        Console.WriteLine(
            "Tar emot sensordata från alla MQTT-enheter...");

        await Task.Delay(Timeout.Infinite);
    }
}
