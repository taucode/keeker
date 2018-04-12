namespace Keeker.Server
{
    public interface IStaticContentResolver
    {
        StaticContentInfo Resolve(string uri);
    }
}
