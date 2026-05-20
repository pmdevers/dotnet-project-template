using System.Reflection;

namespace Template.Api.Domain.Abstractions;

public abstract class AggregateRoot
{
    private EventCollection _events = EventCollection.Empty();

    public static T LoadFromHistory<T>(EventCollection history) 
        where T : AggregateRoot, new()
    {
        var aggregate = new T();
        aggregate.LoadFromHistory(history);
        return aggregate;
    }

    public void LoadFromHistory(EventCollection history)
    {
        _events = history;
        foreach (var e in _events)
        {
            RecordEvent(e, false);
        }
    }

    protected internal void RecordEvent(DomainEvent e)
    {
        ArgumentNullException.ThrowIfNull(e);
        RecordEvent(e, true);
    }

    private void RecordEvent(DomainEvent e, bool isNew)
    {
        ApplyInternal(e);
        if (isNew)
        {
            _events.Append(e);
        }
    }

    private const string _applyMehodName = "Apply";
    private void ApplyInternal(DomainEvent e)
    {
        SafeInvokeMethod(GetType(), this, _applyMehodName, e);
    }

    private static void SafeInvokeMethod(Type type, object target, string name, params object[] args)
    {
        const BindingFlags privateOrPublicMethodFlags = BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        try
        {
            type.InvokeMember(name, privateOrPublicMethodFlags, null, target, args);
        }
        catch (MissingMethodException)
        {
            if (type.BaseType != null)
            {
                SafeInvokeMethod(type.BaseType, target, name, args);
            }
        }
    }

    public EventCollection GetUncommittedEvents()
        => _events.GetUncommittedEvents();
}
