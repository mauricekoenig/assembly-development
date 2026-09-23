using GameEngine;

public sealed class RewardInfo
{
    public RewardType Type { get; }

    public RewardContentType ContentType { get; }

    public string ContentId { get; }

    public string Name { get; }

    public string Description { get; }


    internal RewardInfo(
        RewardType type,
        RewardContentType contentType,
        string contentId,
        string name,
        string description)
    {
        Type = type;
        ContentType = contentType;
        ContentId = contentId ?? string.Empty;
        Name = name ?? string.Empty;
        Description = description ?? string.Empty;
    }


    // Compatibility constructor for older reward implementations.
    internal RewardInfo(
        RewardType type,
        string name,
        string description)
        : this(
            type,
            default,
            string.Empty,
            name,
            description)
    {
    }
}
