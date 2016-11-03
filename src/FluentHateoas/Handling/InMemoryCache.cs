using System.Collections.Generic;
using System.Linq;

namespace FluentHateoas.Handling
{
    public class InMemoryCache<TKey, TValue> : ICache<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _store = new Dictionary<TKey, TValue>();

        public IList<TValue> Get()
        {
            return _store.Values.ToList();
        }

        public TValue Get(TKey key)
        {
            return _store.ContainsKey(key) ? _store[key] : default(TValue);
        }

        public void Add(TKey key, TValue value)
        {
            _store.Add(key, value);
        }
    }
}