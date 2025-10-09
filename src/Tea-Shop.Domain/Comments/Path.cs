namespace Tea_Shop.Domain.Comments;

public sealed record Path
{
    public string Value { get; }

    private const char PathSeparator = '.';

    private Path(string value)
    {
        Value = value;
    }

    public static Path CreateParent(Identifier identifier)
    {
        return new Path(identifier.Value);
    }

    public static Path Create(string value)
    {
        return new Path(value);
    }

    public Path CreateChild(Identifier identifier)
    {
        return new Path(Value + PathSeparator + identifier.Value);
    }
}