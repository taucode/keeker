using Keeker.Core.Streams;
using System;
using System.IO;

namespace Keeker.Core.Relays.ContentRedirectors
{
    public class ChunkedContentRedirector : ContentRedirector
    {
        private readonly KeekStream _sourceStream;
        private readonly AutoBuffer _sourceBuffer;
        private readonly Stream _destinationStream;

        public ChunkedContentRedirector(
            KeekStream sourceStream,
            AutoBuffer sourceBuffer,
            Stream destinationStream)
        {
            _sourceStream = sourceStream;
            _sourceBuffer = sourceBuffer;
            _destinationStream = destinationStream;
        }

        public override void Redirect()
        {
            while (true)
            {
                _sourceStream.ReadInnerStream(20);
                var crlfIndex = _sourceStream.PeekIndexOf(HttpHelper.CrLfBytes);

                if (crlfIndex == -1)
                {
                    throw new ApplicationException(); // todo2[ak] wtf
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
                    throw new ApplicationException(); // todo2[ak]
                }
                totalByteCountRead += byteCountToRead;
                offset += byteCountToRead;

                var lengthOfChunk = _sourceBuffer.Raw.ToAsciiString(0, byteCountActuallyRead).ToIn32FromHex();

                if (lengthOfChunk == 0)
                {
                    // end of chunked content. should read <crlf><crlf> and exit
                    byteCountToRead = HttpHelper.CrLfCrLfBytes.Length;
                    byteCountActuallyRead = _sourceStream.Read(_sourceBuffer.Raw, offset, byteCountToRead);
                    if (byteCountActuallyRead != byteCountToRead)
                    {
                        throw new ApplicationException(); // todo2[ak]
                    }
                    totalByteCountRead += byteCountToRead;
                    offset += byteCountToRead;

                    if (!CoreHelper.ByteArraysEquivalent(
                        _sourceBuffer.Raw,
                        1,
                        HttpHelper.CrLfCrLfBytes,
                        0,
                        HttpHelper.CrLfCrLfBytes.Length))
                    {
                        throw new ApplicationException(); // todo2[ak]
                    }

                    _destinationStream.Write(_sourceBuffer.Raw, 0, totalByteCountRead);
                    break;
                }

                // read crlf
                byteCountToRead = HttpHelper.CrLfBytes.Length;
                byteCountActuallyRead = _sourceStream.Read(_sourceBuffer.Raw, offset, byteCountToRead);
                if (byteCountToRead != byteCountActuallyRead)
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                totalByteCountRead += byteCountToRead;
                offset += byteCountToRead;

                // send length and crlf to destination
                _destinationStream.Write(_sourceBuffer.Raw, 0, totalByteCountRead);

                // redirect chunk
                CoreHelper.RedirectStream(_sourceStream, _destinationStream, _sourceBuffer.Raw, lengthOfChunk);

                // read crlf
                byteCountToRead = HttpHelper.CrLfBytes.Length;
                byteCountActuallyRead = _sourceStream.Read(_sourceBuffer.Raw, 0, byteCountToRead);
                if (byteCountToRead != byteCountActuallyRead)
                {
                    throw new ApplicationException(); // todo2[ak]
                }
                if (!CoreHelper.ByteArraysEquivalent(
                    _sourceBuffer.Raw,
                    0,
                    HttpHelper.CrLfBytes,
                    0,
                    HttpHelper.CrLfBytes.Length))
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                _destinationStream.Write(_sourceBuffer.Raw, 0, byteCountActuallyRead);
            }
        }
    }
}