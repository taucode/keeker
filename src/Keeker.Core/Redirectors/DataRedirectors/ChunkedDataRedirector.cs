using Keeker.Core.Relays;
using Keeker.Core.Streams;
using System;
using System.IO;

namespace Keeker.Core.Redirectors.DataRedirectors
{
    public class ChunkedDataRedirector : DataRedirector
    {
        private KeekStream sourceStream;
        private AutoBuffer sourceBuffer;
        private Stream destinationStream;

        public ChunkedDataRedirector(
            KeekStream sourceStream,
            AutoBuffer sourceBuffer,
            Stream destinationStream)
        {
            this.sourceStream = sourceStream;
            this.sourceBuffer = sourceBuffer;
            this.destinationStream = destinationStream;
        }

        public override void Redirect()
        {
            while (true)
            {
                sourceStream.ReadInnerStream(20);
                var crlfIndex = sourceStream.PeekIndexOf(HttpHelper.CrLfBytes);

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
                byteCountActuallyRead = sourceStream.Read(sourceBuffer.Raw, offset, byteCountToRead);
                if (byteCountToRead != byteCountActuallyRead)
                {
                    throw new ApplicationException(); // todo2[ak]
                }
                totalByteCountRead += byteCountToRead;
                offset += byteCountToRead;

                var lengthOfChunk = sourceBuffer.Raw.ToAsciiString(0, byteCountActuallyRead).ToIn32FromHex();

                if (lengthOfChunk == 0)
                {
                    // end of chunked content. should read <crlf><crlf> and exit
                    byteCountToRead = HttpHelper.CrLfCrLfBytes.Length;
                    byteCountActuallyRead = sourceStream.Read(sourceBuffer.Raw, offset, byteCountToRead);
                    if (byteCountActuallyRead != byteCountToRead)
                    {
                        throw new ApplicationException(); // todo2[ak]
                    }
                    totalByteCountRead += byteCountToRead;
                    offset += byteCountToRead;

                    if (!CoreHelper.ByteArraysEquivalent(
                        sourceBuffer.Raw,
                        1,
                        HttpHelper.CrLfCrLfBytes,
                        0,
                        HttpHelper.CrLfCrLfBytes.Length))
                    {
                        throw new ApplicationException(); // todo2[ak]
                    }

                    destinationStream.Write(sourceBuffer.Raw, 0, totalByteCountRead);
                    break;
                }

                // read crlf
                byteCountToRead = HttpHelper.CrLfBytes.Length;
                byteCountActuallyRead = sourceStream.Read(sourceBuffer.Raw, offset, byteCountToRead);
                if (byteCountToRead != byteCountActuallyRead)
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                totalByteCountRead += byteCountToRead;
                offset += byteCountToRead;

                // send length and crlf to destination
                destinationStream.Write(sourceBuffer.Raw, 0, totalByteCountRead);

                // redirect chunk
                //_chunkRedirector.Redirect(lengthOfChunk);
                CoreHelper.RedirectStream(sourceStream, destinationStream, sourceBuffer.Raw, lengthOfChunk);

                // read crlf
                byteCountToRead = HttpHelper.CrLfBytes.Length;
                byteCountActuallyRead = sourceStream.Read(sourceBuffer.Raw, 0, byteCountToRead);
                if (byteCountToRead != byteCountActuallyRead)
                {
                    throw new ApplicationException(); // todo2[ak]
                }
                if (!CoreHelper.ByteArraysEquivalent(
                    sourceBuffer.Raw,
                    0,
                    HttpHelper.CrLfBytes,
                    0,
                    HttpHelper.CrLfBytes.Length))
                {
                    throw new ApplicationException(); // todo2[ak]
                }

                destinationStream.Write(sourceBuffer.Raw, 0, byteCountActuallyRead);
            }
        }
    }
}