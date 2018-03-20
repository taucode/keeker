namespace Keeker.Core.Relays
{
    public enum StreamRedirectorState
    {
        NotStarted = 0,
        Idle,
        Metadata,
        Data,
    }
}
