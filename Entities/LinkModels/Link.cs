namespace Entities.LinkModels;

public sealed class Link(string href, string rel, string method)
{
    public string? Href { get; init; } = href;
    public string? Rel { get; init; } = rel;
    public string? Method { get; init; } = method;
}