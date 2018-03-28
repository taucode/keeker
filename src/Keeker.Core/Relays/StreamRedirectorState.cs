namespace Keeker.Core.Relays
{
    public enum StreamRedirectorState
    {
        Unknown = 0,
        Idle,
        Metadata,
        Data,
        Stop,
    }
}
