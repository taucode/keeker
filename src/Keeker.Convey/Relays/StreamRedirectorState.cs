namespace Keeker.Convey.Relays
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
