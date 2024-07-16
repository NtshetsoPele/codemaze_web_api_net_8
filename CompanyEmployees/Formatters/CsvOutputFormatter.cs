namespace CompanyEmployees.Formatters;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(Resources.CsvFormat));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }
    
    public override async Task WriteResponseBodyAsync(
        OutputFormatterWriteContext context, Encoding encoding)
    {
        var buffer = new StringBuilder();
        if (context.Object is IEnumerable<ToClientCompany> companies)
        {
            foreach (var company in companies)
            {
                FormatCsv(buffer, company);
            }
        }
        else
        {
            FormatCsv(buffer, (ToClientCompany)context.Object!);
        }
        await context.HttpContext.Response.WriteAsync(buffer.ToString());
    }

    private static void FormatCsv(StringBuilder buffer, ToClientCompany company)
    {
        buffer.AppendLine($"\"{company.CompanyId}\", \"{company.Name}\", \"{company.FullAddress}\"");
    }
    
    protected override bool CanWriteType(Type? type)
    {
        if (typeof(ToClientCompany).IsAssignableFrom(type) ||
            typeof(IEnumerable<ToClientCompany>).IsAssignableFrom(type))
        {
            return base.CanWriteType(type);
        }
        return false;
    }
}