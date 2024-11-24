namespace PublicacaoMetricas.Common;

public abstract class Message
{
    protected Guid Id { get; } = Guid.NewGuid();
    protected DateTime CreatedAt { get; } = DateTime.UtcNow;
}