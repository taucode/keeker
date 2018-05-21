namespace Keeker.Server
{
    //public class RequestMetadataReader
    //{
    //    private const int PORTION_SIZE = 65530;
    //    private const int TIMEOUT_MILLISECONDS = 1;

    //    private readonly KeekStream _stream;
    //    private readonly AutoBuffer _buffer;
    //    private readonly ManualResetEvent _stopSignal;
    //    private readonly TimeSpan _timeout;

    //    public RequestMetadataReader(KeekStream stream, ManualResetEvent stopSignal)
    //    {
    //        _stream = stream;
    //        _buffer = new AutoBuffer();
    //        _stopSignal = stopSignal;
    //        _timeout = TimeSpan.FromMilliseconds(TIMEOUT_MILLISECONDS);
    //    }

    //    public HttpRequestMetadata Read()
    //    {
    //        while (true)
    //        {
    //            var bytesReadCount = _stream.ReadInnerStream(PORTION_SIZE);

    //            if (bytesReadCount == 0)
    //            {
    //                // no data yet, let's wait for a while
    //                var gotSignal = _stopSignal.WaitOne(_timeout);
    //                if (gotSignal)
    //                {
    //                    return null;
    //                }
    //            }
    //            else
    //            {
    //                var metadataLength = this.GetMetadataLength();
    //                if (metadataLength >= 0)
    //                {
    //                    _buffer.Allocate(metadataLength);
    //                    _stream.Read(_buffer.Raw, 0, metadataLength);

    //                    var metadata = HttpRequestMetadata.Parse(_buffer.Raw, 0);
    //                    return metadata;
    //                }
    //            }
    //        }
    //    }

    //    private int GetMetadataLength()
    //    {
    //        var index = _stream.PeekIndexOf(CoreHelper.CrLfCrLfBytes);
    //        if (index == -1)
    //        {
    //            return -1;
    //        }

    //        var metadataLength = index + CoreHelper.CrLfCrLfBytes.Length;
    //        return metadataLength;
    //    }
    //}
}