
public class TelemetryDisplayService
{
    public void Display(TelemetryMessage message)
    {
        Console.WriteLine(
            $"[{message.Timestamp:yyyy-MM-dd HH:mm:ss}] " +
            $"{message.DeviceId} | " +
            $"{message.Value} {message.Unit}");
    }
}
