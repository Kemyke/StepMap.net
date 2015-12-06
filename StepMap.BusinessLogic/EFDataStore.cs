using Google.Apis.Util.Store;
using Newtonsoft.Json;
using StepMap.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.BusinessLogic
{
    public class EFDataStore : IDataStore
    {
        public async Task ClearAsync()
        {
            using (var ctx = new StepMapDbContext())
            {
                ctx.DataStores.RemoveRange(ctx.DataStores);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(key);
            }

            using (var ctx = new StepMapDbContext())
            {
                var generatedKey = GenerateStoredKey(key, typeof(T));
                var item = ctx.DataStores.FirstOrDefault(x => x.Key == generatedKey);
                if (item != null)
                {
                    ctx.DataStores.Remove(item);
                    await ctx.SaveChangesAsync();
                }
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(key);
            }

            using (var ctx = new StepMapDbContext())
            {
                var generatedKey = GenerateStoredKey(key, typeof(T));
                var item = ctx.DataStores.FirstOrDefault(x => x.Key == generatedKey);
                T value = item == null ? default(T) : JsonConvert.DeserializeObject<T>(item.Value);
                return value;
            }
        }

        public async Task StoreAsync<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(key);
            }

            using (var ctx = new StepMapDbContext())
            {
                var generatedKey = GenerateStoredKey(key, typeof(T));
                string json = JsonConvert.SerializeObject(value);

                var item = ctx.DataStores.SingleOrDefault(x => x.Key == generatedKey);

                if (item == null)
                {
                    ctx.DataStores.Add(new DataStore { Key = generatedKey, Value = json });
                }
                else
                {
                    item.Value = json;
                }

                await ctx.SaveChangesAsync();
            }
        }

        private static string GenerateStoredKey(string key, Type t)
        {
            return string.Format("{0}-{1}", t.FullName, key);
        }
    }
}
