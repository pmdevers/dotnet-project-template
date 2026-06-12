namespace Template.Api.Configuration;

public class EventBusOptions
{
    public bool ReadFromBeginning { get; set; }
    public required string StreamName { get; set; }
    public required string ConsumerGroup { get; set; }
    public required string ConsumerName { get; set; }
}
