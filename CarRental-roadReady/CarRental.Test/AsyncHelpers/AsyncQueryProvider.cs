using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CarRental.Test.AsyncHelpers
{
    public class AsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        // Constructor
        public AsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        // Create query methods
        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerable<TElement>(expression);
        }

        // Execute methods
        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        // Asynchronous query execution (ExecuteAsync)
        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new AsyncEnumerable<TResult>(expression);
        }

        // Execute asynchronously with cancellation token
        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            // Perform your query execution asynchronously (just an example)
            var result = Execute<TResult>(expression);  // Execute synchronously

            // You can perform any async task you want here
            // For the sake of example, we just return the result directly.
            return result;
        }
    }
}
