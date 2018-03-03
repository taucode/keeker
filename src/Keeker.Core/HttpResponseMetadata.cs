﻿using System.IO;

namespace Keeker.Core
{
    public class HttpResponseMetadata
    {
        public HttpResponseMetadata(HttpStatusLine line, HttpHeaderCollection headers)
        {
            this.Line = line;
            this.Headers = headers;
        }

        public HttpStatusLine Line { get; }

        public HttpHeaderCollection Headers { get; }

        public byte[] ToArray()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteAll(this.Line.ToArray());
                stream.WriteAll(this.Headers.ToArray());

                stream.WriteAll(HttpHelper.CrLfBytes);

                return stream.ToArray();
            }
        }

        public static HttpResponseMetadata Parse(byte[] buffer, int start)
        {
            // parse status line
            var line = HttpStatusLine.Parse(buffer, 0);
            start += line.ByteCount;

            // parse headers
            var headers = HttpHeaderCollection.Parse(buffer, start);

            var metadata = new HttpResponseMetadata(line, headers);

            return metadata;
        }
    }
}