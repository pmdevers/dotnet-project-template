namespace Template.Api.Domain.Abstractions;

public record PagedResults<T>
{
    public IEnumerable<T> Items { get; }
    public int TotalCount { get; }
    public int PageSize { get; }
    public int TotalPages { get; }

    public PagedResults(IEnumerable<T> items, int totalCount, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(items);
        if (totalCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalCount), "Total count cannot be negative.");
        }
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        TotalPages = Math.Max((int)Math.Ceiling((double)totalCount / PageSize), 1);
    }
}
