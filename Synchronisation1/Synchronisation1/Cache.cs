using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Synchronisation1
{
    
    internal class Cache
    {
        public ReaderWriterLockSlim CacheLock = new ReaderWriterLockSlim();
        public Dictionary<string,int> container = new Dictionary<string, int>();
        public int? Get(string key)
        {
            CacheLock.EnterReadLock();
            bool exists = container.TryGetValue(key,out int value);
            if (exists)
            {
                CacheLock.ExitReadLock();
                return value;
            }
            else
            {
                CacheLock.ExitReadLock();
                return null;
            }
        }
        public void AddOrUpdate(string key , int value)
        {
            CacheLock.EnterWriteLock();
            if (container.ContainsKey(key))
            {
                container[key] = value;
                CacheLock.ExitWriteLock();
            }
            else
            {
                container.Add(key,value);
                CacheLock.ExitWriteLock();
            }
        }

        public int GetOrAdd(string key , Func<int> valueFactory)
        {
            CacheLock.EnterUpgradeableReadLock();
            if (container.ContainsKey(key))
            {
                CacheLock.ExitUpgradeableReadLock();
                return container[key];
            }
            else
            {
                CacheLock.EnterWriteLock();
                int value = valueFactory();
                container.Add(key, value);
                CacheLock.ExitWriteLock();
                return value;
            }            
        }
    }
}
