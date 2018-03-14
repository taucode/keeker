using System;
using System.IO;

namespace Keeker.Core.Relays
{
    public class ChunkedContentRedirector
    {
        private readonly KeekStream _sourceStream;
        private readonly Stream _destinationStream;
        private readonly AutoBuffer _buffer;
        private readonly FixedSizeContentRedirector _chunkRedirector;

        public ChunkedContentRedirector(
            KeekStream sourceStream,
            Stream destinationStream,
            AutoBuffer buffer)
        {
            _sourceStream = sourceStream;
            _destinationStream = destinationStream;
            _buffer = buffer;
            _chunkRedirector = new FixedSizeContentRedirector(_sourceStream, _destinationStream, buffer);
        }

        public void Redirect()
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
                byteCountActuallyRead = _sourceStream.Read(_buffer.Buffer, offset, byteCountToRead);
                if (byteCountToRead != byteCountActuallyRead)
                {
                    throw new ApplicationException(); // todo2[ak]
                }
                totalByteCountRead += byteCountToRead;
                offset += byteCountToRead;

                var lengthOfChunk = _buffer.Buffer.ToAsciiString(0, byteCountActuallyRead).ToIn32FromHex();

                if (lengthOfChunk == 0)
                {
                    // end of chunked content. should read <crlf><crlf> and exit
                    byteCountToRead = HttpHelper.CrLfCrLfBytes.Length;
                    byteCountActuallyRead = _sourceStream.Read(_buffer.Buffer, offset, byteCountToRead);
                    if (byteCountActuallyRead != byteCountToRead)
                    {
                        throw new ApplicationException(); // todo2[ak]
                    }
                    totalByteCountRead += byteCountToRead;
                    offset += byteCountToRead;

                    if (!CoreHelper.ByteArraysEquivalent(
                        _buffer.Buffer,
                        1,
                        HttpHelper.CrLfCrLfBytes,
                        0,
                        HttpHelper.CrLfCrLfBytes.Length))
                    {
                        throw new ApplicationException(); // todo2[ak]
                    }

                    _destinationStream.Write(_buffer.Buffer, 0, totalByteCountRead);
                    break;
                }

                // read crlf
                byteCountToRead = HttpHelper.CrLfBytes.Length;
                byteCountActuallyRead = _sourceStream.Read(_buffer.Buffer, offset, byteCountToRead);
                if (byteCountToRead != byteCountActuallyRead)
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                totalByteCountRead += byteCountToRead;
                offset += byteCountToRead;

                // send length and crlf to destination
                _destinationStream.Write(_buffer.Buffer, 0, totalByteCountRead);

                // redirect chunk
                _chunkRedirector.Redirect(lengthOfChunk);

                // read crlf
                byteCountToRead = HttpHelper.CrLfBytes.Length;
                byteCountActuallyRead = _sourceStream.Read(_buffer.Buffer, 0, byteCountToRead);
                if (byteCountToRead != byteCountActuallyRead)
                {
                    throw new ApplicationException(); // todo2[ak]
                }
                if (!CoreHelper.ByteArraysEquivalent(
                    _buffer.Buffer,
                    0,
                    HttpHelper.CrLfBytes,
                    0,
                    HttpHelper.CrLfBytes.Length))
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                _destinationStream.Write(_buffer.Buffer, 0, byteCountActuallyRead);
            }
        }
    }
}
