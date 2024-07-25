namespace Repository.Extensions;

public static class RepositoryEmployeeExtensions
{
    public static IQueryable<Employee> Filter(this IQueryable<Employee> employees, uint minAge, uint maxAge)
    {
        return employees.Where((Employee e) => e.Age >= minAge && e.Age <= maxAge);
    }

    public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return employees;
        }
        string lowerCaseTerm = searchTerm.Trim().ToLower();
        return employees.Where((Employee e) => e.Name.Contains(lowerCaseTerm));
    }

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            return employees.OrderBy((Employee e) => e.Name);
        }
        
        string orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);
        
        if (string.IsNullOrWhiteSpace(orderQuery))
        {
            return employees.OrderBy((Employee e) => e.Name);
        }
        return employees.OrderBy(orderQuery);
    }
    
    public static IQueryable<Employee> Paginate(
        this IQueryable<Employee> employees, int pageNumber, int pageSize)
    {
        return 
            employees
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
    }
}