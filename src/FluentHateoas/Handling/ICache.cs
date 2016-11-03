using System.Collections.Generic;

namespace FluentHateoas.Handling
{
    public interface ICache<in TKey, TValue>
    {
        IList<TValue> Get();
        TValue Get(TKey key);
        void Add(TKey key, TValue value);
    }
}