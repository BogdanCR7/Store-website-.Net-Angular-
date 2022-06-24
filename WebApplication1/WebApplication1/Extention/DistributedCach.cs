using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApplication1.Extention
{
    public static class DistributedCach
    {
        public static async Task SetRecordAsync<T>
            (this IDistributedCache cache,string recordId,T data,
            TimeSpan? expiretime, TimeSpan? unusedtime)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = expiretime ?? TimeSpan.FromMinutes(60);
            options.SlidingExpiration = unusedtime;
            var jsondata = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsondata, options);
        }
        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache,
            string recordId)
        {
            var jsondata = await cache.GetStringAsync(recordId);
            if(jsondata is null)
            {
                return default (T);
            }
            return JsonSerializer.Deserialize<T>(jsondata);
        }
    }
}
