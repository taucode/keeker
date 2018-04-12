using Keeker.Core;
using Keeker.Core.Exceptions;
using Keeker.Core.Streams;
using System.IO;
using System.Threading;

namespace Keeker.Convey.Relays.ContentRedirectors
{
    public class ChunkedContentRedirector : ContentRedirector
    {
        private readonly ManualResetEvent _signal;
        private readonly KeekStream _sourceStream;
        private readonly AutoBuffer _sourceBuffer;
        private readonly Stream _destinationStream;

        public ChunkedContentRedirector(
            ManualResetEvent signal,
            KeekStream sourceStream,
            AutoBuffer sourceBuffer,
            Stream destinationStream)
        {
            _signal = signal;
            _sourceStream = sourceStream;
            _sourceBuffer = sourceBuffer;
            _destinationStream = destinationStream;
        }

        private BadHttpDataException CreateException()
        {
            return new BadHttpDataException("Bad chunked HTTP stream");
        }

        public override void Redirect()
        {
            while (true)
            {
                var gotSignal = _signal.WaitOne(0);
                if (gotSignal)
                {
                    break; // bye
                }

                _sourceStream.ReadInnerStream(20);
                var crlfIndex = _sourceStream.PeekIndexOf(CoreHelper.CrLfBytes);

                if (crlfIndex == -1)
                {
                    throw this.CreateException();
                }

                int byteCountToRead;
                int byteCountActuallyRead;
                var offset = 0;
                var totalByteCountRead = 0;

                // read hex length
                byteCountToRead = crlfIndex;
                byteCountActuallyRead = _sourceStream.Read(_sourceBuffer.Raw, offset, byteCountToRead);
                if (byteCountToRead != byteCountActuallyRead)
                {
                    throw this.CreateException();
                }
                totalByteCountRead += byteCountToRead;
                offset += byteCountToRead;

                var lengthOfChunk = _sourceBuffer.Raw.ToAsciiString(0, byteCountActuallyRead).ToIn32FromHex();

                if (lengthOfChunk == 0)
                {
                    // end of chunked content. should read <crlf><crlf> and exit
                    byteCountToRead = CoreHelper.CrLfCrLfBytes.Length;
                    byteCountActuallyRead = _sourceStream.Read(_sourceBuffer.Raw, offset, byteCountToRead);
                    if (byteCountActuallyRead != byteCountToRead)
                    {
                        throw new BadHttpDataException(); // todo2[ak]
                    }
                    totalByteCountRead += byteCountToRead;
                    offset += byteCountToRead;

                    if (!CoreHelper.ByteArraysEquivalent(
                        _sourceBuffer.Raw,
                        1,
                        CoreHelper.CrLfCrLfBytes,
                        0,
                        CoreHelper.CrLfCrLfBytes.Length))
                    {
                        throw this.CreateException();
                    }

                    _destinationStream.Write(_sourceBuffer.Raw, 0, totalByteCountRead);
                    break;
                }

                // read crlf
                byteCountToRead = CoreHelper.CrLfBytes.Length;
                byteCountActuallyRead = _sourceStream.Read(_sourceBuffer.Raw, offset, byteCountToRead);
                if (byteCountToRead != byteCountActuallyRead)
                {
                    throw this.CreateException();
                }

                totalByteCountRead += byteCountToRead;
                offset += byteCountToRead;

                // send length and crlf to destination
                _destinationStream.Write(_sourceBuffer.Raw, 0, totalByteCountRead);

                // redirect chunk
                CoreHelper.RedirectStream(_signal, _sourceStream, _destinationStream, _sourceBuffer.Raw, lengthOfChunk);

                // read crlf
                byteCountToRead = CoreHelper.CrLfBytes.Length;
                byteCountActuallyRead = _sourceStream.Read(_sourceBuffer.Raw, 0, byteCountToRead);
                if (byteCountToRead != byteCountActuallyRead)
                {
                    throw this.CreateException();
                }
                if (!CoreHelper.ByteArraysEquivalent(
                    _sourceBuffer.Raw,
                    0,
                    CoreHelper.CrLfBytes,
                    0,
                    CoreHelper.CrLfBytes.Length))
                {
                    throw this.CreateException();
                }

                _destinationStream.Write(_sourceBuffer.Raw, 0, byteCountActuallyRead);
            }
        }
    }
}