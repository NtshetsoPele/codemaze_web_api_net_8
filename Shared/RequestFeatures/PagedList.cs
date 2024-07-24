namespace Shared.RequestFeatures;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; init; }

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        AddRange(items);
        
        MetaData = new MetaData
        {
            TotalCount = count,
            CurrentPage = pageNumber,
            PageSize = pageSize, 
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };
    }

    public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var sourceList = source.ToList();
        var count = sourceList.Count;
        var items = sourceList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}