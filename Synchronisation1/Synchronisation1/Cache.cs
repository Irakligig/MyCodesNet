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
        public static ReaderWriterLockSlim CacheLock = new ReaderWriterLockSlim();
        public static Dictionary<string,int> container = new Dictionary<string, int>();
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

        }

        public int GetOrAdd(string key , Func<int> valueFactory)
        {

        }
    }
}
