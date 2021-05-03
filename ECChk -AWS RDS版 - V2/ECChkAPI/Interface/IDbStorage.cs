using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECChkAPI.Interface
{
    public interface IDbStorage
    {
        Task<IEnumerable<T>> GetCacheByStoreno<T>(string storeno) where T : class, new();
        Task<bool> SetCacheByStoreNo(string storeKey, string value);
        Task<bool> DeleteKey(string storeNo);
    }

}
