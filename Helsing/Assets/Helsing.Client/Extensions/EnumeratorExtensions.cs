using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Helsing.Client.Extensions
{
    public static class EnumeratorExtensions
    {
        public static async Task AsyncForEach<T>(this IEnumerable<T> collection, Func<T, Task> func)
        {
            foreach (var value in collection)
            {
                await func(value);
            }
        }
    }
}
