namespace Template.Api.Domain.Abstractions;

public class EventCollection : IEnumerable<DomainEvent>
{
    private DomainEvent[] _events = [];
    private readonly List<DomainEvent> _uncomitted = [];
    private TimeProvider _timeProvider;
    private EventCollection(DomainEvent[]? events, TimeProvider? timeProvider = null)
    {
        _events = events ?? [];
        _timeProvider = timeProvider ?? TimeProvider.System;
    }

    public int Version => _events.Length;
    public int ExpectedVersion => _events.Length + _uncomitted.Count;

    /// <summary>
    /// The creation date of this event stream.
    /// </summary>
    public DateTimeOffset? CreatedOn
        => _events.Length != 0
        ? _events.FirstOrDefault()?.OccurredOn
        : _uncomitted.FirstOrDefault()?.OccurredOn;

    /// <summary>
    /// The date of the last modification.
    /// </summary>
    public DateTimeOffset? LastModifiedOn
        => _uncomitted.Count != 0
        ? _uncomitted.LastOrDefault()?.OccurredOn
        : _events.LastOrDefault()?.OccurredOn;

    public void Append(DomainEvent @event) 
        => _uncomitted.Add(@event with { OccurredOn = _timeProvider.GetUtcNow() });

    public static EventCollection Empty(TimeProvider? timeProvider = null) => new(null, timeProvider);

    public static EventCollection From(DomainEvent[]? events = null, TimeProvider? timeProvider = null) 
        => new(events, timeProvider);

    public EventCollection GetUncommittedEvents()
        => From([.. _uncomitted], _timeProvider);

    public void ClearUncommittedEvents()
        => _uncomitted.Clear();

    public IEnumerator<DomainEvent> GetEnumerator()
        => _events.ToList().GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        => GetEnumerator();
}
