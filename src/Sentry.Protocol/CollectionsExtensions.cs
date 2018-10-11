#if !LACKS_CONCURRENT_COLLECTIONS
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;

namespace Sentry.Protocol
{
    internal static class CollectionsExtensions
    {
        public static TValue GetOrCreate<TValue>(
#if !LACKS_CONCURRENT_COLLECTIONS
            this ConcurrentDictionary<string, object> dictionary,
#else
            this Dictionary<string, object> dictionary,
#endif
            string key)
            where TValue : class, new()
#if !LACKS_CONCURRENT_COLLECTIONS
            => dictionary.GetOrAdd(key, _ => new TValue()) as TValue;
#else
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value as TValue;
            }
            else
            {
                value = new TValue();
                dictionary[key] = value;
                return value as TValue;
            }
        }
#endif

        public static void TryCopyTo<TKey, TValue>(this IDictionary<TKey, TValue> from, IDictionary<TKey, TValue> to)
        {
            if (from == null || to == null)
            {
                return;
            }

            foreach (var kv in from)
            {
                if (!to.ContainsKey(kv.Key))
                {
                    to[kv.Key] = kv.Value;
                }
            }
        }
    }
}
