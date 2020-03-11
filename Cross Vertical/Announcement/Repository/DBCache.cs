using CrossVertical.Announcement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossVertical.Announcement.Repository
{
    public static class Cache
    {
        public static DBCache<Tenant> Tenants { get; set; } = new DBCache<Tenant>();
        public static DBCache<Group> Groups { get; set; } = new DBCache<Group>();
        public static DBCache<Team> Teams { get; set; } = new DBCache<Team>();
        public static DBCache<User> Users { get; set; } = new DBCache<User>();
        public static DBCache<Campaign> Announcements { get; set; } = new DBCache<Campaign>();
    }

    public class DBCache<T> where T : DatabaseItem
    {
        private Dictionary<string, T> CachedItems { get; set; } = new Dictionary<string, T>();

        public async Task<List<T>> GetAllItemsAsync()
        {
            if (CachedItems.Count == 0) // Try to fetch from DB.
            {
                var items = await DocumentDBRepository.GetItemsAsync<T>(u => u.Type == typeof(T).Name);
                foreach (T item in items)
                {
                    CachedItems.Add(item.Id, item);
                }
            }
            return CachedItems.Values.ToList();
        }

        public async Task<T> GetItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            if (CachedItems.ContainsKey(id))
                return CachedItems[id];
            else
            {
                var tenant = await DocumentDBRepository.GetItemAsync<T>(id);
                if (tenant != null)
                {
                    try
                    {
                        CachedItems.Add(id, tenant);
                    }
                    catch (System.Exception ex)
                    {
                        Helpers.ErrorLogService.LogError(ex);
                    }
                    return tenant;
                }
                return null; // Not found in DB.
            }
        }

        public async Task AddOrUpdateItemAsync(string id, T item)
        {
            if (CachedItems.ContainsKey(id))
            {
                // Update Existing
                await DocumentDBRepository.UpdateItemAsync(id, item);
                CachedItems[id] = item;
            }
            else
            {
                await DocumentDBRepository.CreateItemAsync(item);
                CachedItems.Add(id, item);
            }
        }

        public async Task DeleteItemAsync(string id)
        {
            if (CachedItems.ContainsKey(id))
            {
                // Update Existing
                await DocumentDBRepository.DeleteItemAsync(id);
                CachedItems.Remove(id);
            }
            else
            {
                await DocumentDBRepository.DeleteItemAsync(id);
            }
        }
    }
}