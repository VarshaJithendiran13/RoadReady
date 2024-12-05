using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Test.AsyncHelpers
{

    public class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public AsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public T Current => _inner.Current;

        public async ValueTask DisposeAsync()
        {
            await Task.CompletedTask;
            _inner.Dispose();
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            return await Task.FromResult(_inner.MoveNext());
        }
    }

}
