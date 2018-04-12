using System;
using System.Threading;

namespace Keeker.Core
{
    public class IdGenerator
    {
        private int _next;

        public IdGenerator(string prefix)
        {
            this.Prefx = prefix ?? throw new ArgumentNullException(nameof(prefix));
        }

        public IdGenerator()
            : this(string.Empty)
        {
        }

        public string Prefx { get; }

        public string Generate()
        {
            var next = Interlocked.Increment(ref _next);
            var id = $"{this.Prefx}{next}";
            return id;
        }
    }
}