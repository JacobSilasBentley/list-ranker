using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListRanker.Application
{
    public class DataStore : DbContext, IDataStore
    {
        DbSet<ItemPreference> ItemPreferences { get; set; }
        DbSet<ListItem> ListItems { get; set; }

        public Task<List<ItemPreference>> GetItemPreferences()
        {
            return ItemPreferences
                .Include(i => i.Item1)
                .Include(i => i.Item2)
                .ToListAsync();
            //
        }

        public Task<(ListItem, ListItem)> GetTwoRandomItems()
        {
            var values = ListItems.ToList();
            var selectedPair = values.OrderBy(i => Guid.NewGuid()).Take(2).ToArray();
            return Task.FromResult((selectedPair[0], selectedPair[1]));
        }

        public bool InitialisationRequired()
        {
            Database.EnsureCreated();
            return !ListItems.Any();
        }

        public Task InitialiseDatabase(List<string> values)
        {
            Database.EnsureCreated();
            var listItems = values.Select(i => new ListItem(i));
            ListItems.AddRange(listItems);
            return SaveChangesAsync();
        }

        public Task ProvidePreference(ItemPreference itemPreference)
        {
            ListItem item1 = ListItems.Find(itemPreference.Item1.ID);
            ListItem item2 = ListItems.Find(itemPreference.Item2.ID);
            itemPreference.Item1 = item1;
            itemPreference.Item2 = item2;
            ItemPreferences.Add(itemPreference);
            return SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("db-connection"));
        }
    }
}
