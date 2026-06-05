namespace Template.Api.Domain.Abstractions;

public record PagedResults<T>
{
    public IEnumerable<T> Items { get; }
    public int TotalCount { get; }
    public int PageCount { get; }
    public int PageSize { get; }
    public int TotalPages { get; }

    public PagedResults(IEnumerable<T> items, int totalCount, int pageCount, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(items);
        if (totalCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalCount), "Total count cannot be negative.");
        }
        Items = items;
        TotalCount = totalCount;
        PageCount = pageCount;
        PageSize = pageSize;
        TotalPages =Math.Max((int)Math.Ceiling((double)totalCount / PageSize), 1);
    }
}
