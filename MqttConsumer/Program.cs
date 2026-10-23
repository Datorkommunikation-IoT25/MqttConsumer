
var displayService = new TelemetryDisplayService();
var mqttConsumerService = new MqttConsumerService(displayService);

await mqttConsumerService.StartAsync();
