namespace Entities.ErrorModel;

public sealed class ErrorDetails
{
    public required string? Message { get; init; }
        
    public override string ToString() => JsonSerializer.Serialize(this);
}