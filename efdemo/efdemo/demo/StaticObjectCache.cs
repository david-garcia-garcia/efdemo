using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace ETG.SABENTISpro.Application.Core.Module.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class StaticObjectCache : ObjectCache, IEnumerable, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected ConcurrentDictionary<string, object> Data = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        private const DefaultCacheCapabilities Capabilities = DefaultCacheCapabilities.InMemoryProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public StaticObjectCache()
        {
        }

        /// <inheritdoc cref="MemoryCache"/>
        public override DefaultCacheCapabilities DefaultCacheCapabilities
        {
            get
            {
                return Capabilities;
            }
        }

        /// <inheritdoc cref="MemoryCache"/>
        public override string Name
        {
            get { return "Name"; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override object this[string key]
        {
            get
            {
                return this.Data[key];
            }

            set
            {
                this.Set(key, value, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public override CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(IEnumerable<string> keys, string regionName = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Data.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public override bool Contains(string key, string regionName = null)
        {
            return this.Data.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public override bool Add(CacheItem item, CacheItemPolicy policy)
        {
            return this.Data.TryAdd(item.Key, item.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public override object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            if (!string.IsNullOrWhiteSpace(regionName))
            {
                throw new NotSupportedException("Region name not supported");
            }

            this.Data.TryAdd(key, value);
            return this.Data[key];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public override CacheItem AddOrGetExisting(CacheItem item, CacheItemPolicy policy)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return new CacheItem(item.Key, this.AddOrGetExisting(item.Key, item.Value, policy));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="policy"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public override object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            if (!string.IsNullOrWhiteSpace(regionName))
            {
                throw new NotSupportedException("Region name not supported");
            }

            return this.AddOrGetExisting(key, value, policy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public override object Get(string key, string regionName = null)
        {
            return this.Data[key];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public override CacheItem GetCacheItem(string key, string regionName = null)
        {
            object value = this.Get(key, regionName);
            return (value != null) ? new CacheItem(key, value) : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        /// <param name="regionName"></param>
        public override void Set(string key, object value, DateTimeOffset absoluteExpiration, string regionName = null)
        {
            if (!string.IsNullOrWhiteSpace(regionName))
            {
                throw new NotSupportedException("Region name not supported");
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = absoluteExpiration;
            this.Set(key, value, policy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="policy"></param>
        public override void Set(CacheItem item, CacheItemPolicy policy)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.Set(item.Key, item.Value, policy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="policy"></param>
        /// <param name="regionName"></param>
        public override void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            if (!string.IsNullOrWhiteSpace(regionName))
            {
                throw new NotSupportedException("Region name not supported");
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            this.AddOrUpdate(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public override object Remove(string key, string regionName = null)
        {
            return this.Remove(key, CacheEntryRemovedReason.Removed, regionName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reason"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public object Remove(string key, CacheEntryRemovedReason reason, string regionName = null)
        {
            if (!string.IsNullOrWhiteSpace(regionName))
            {
                throw new NotSupportedException("Region name not supported");
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            object value;
            this.Data.TryRemove(key, out value);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public override long GetCount(string regionName = null)
        {
            return this.Data.Keys.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public override IDictionary<string, object> GetValues(IEnumerable<string> keys, string regionName = null)
        {
            if (!string.IsNullOrWhiteSpace(regionName))
            {
                throw new NotSupportedException("Region name not supported");
            }

            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            Dictionary<string, object> values = null;

            foreach (string key in keys)
            {
                if (key == null)
                {
                    throw new ArgumentException("Collection contains null element: key");
                }

                object value = this.Get(key, null);

                if (value != null)
                {
                    if (values == null)
                    {
                        values = new Dictionary<string, object>();
                    }

                    values[key] = value;
                }
            }

            return values;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return this.Data.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void AddOrUpdate(string key, object value)
        {
            this.Data.AddOrUpdate(
                key,
                (theKey) => value,
                (theKey, oldValue) => value);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Data = null;
            }
        }
    }
}
