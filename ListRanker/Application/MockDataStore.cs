using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ListRanker.Application
{
    public class DisneyFilm
    {
        public string movie_title { get; set; }
        public string release_date { get; set; }
        public string genre { get; set; }
        public string mpaa_rating { get; set; }
        public double total_gross { get; set; }
        public double inflation_adjusted_gross { get; set; }

        public override string ToString()
        {
            return $"{movie_title}";
        }
    }

    public class MockDataStore : IDataStore
    {
        private List<string> items;
        private IEnumerable<ListItem> listItems => items.Select((x, index) => new ListItem(x) { ID = index + 1 });

        private readonly List<ItemPreference> itemPreferences = new List<ItemPreference>();

        public MockDataStore()
        {
            
        }

        public Task<List<ItemPreference>> GetItemPreferences()
        {
            return Task.FromResult(itemPreferences);
        }

        public Task<(ListItem, ListItem)> GetTwoRandomItems()
        {
            var toOutput = listItems
                .OrderBy(i => Guid.NewGuid())
                .Take(2)
                .ToList();
            return Task.FromResult((toOutput[0], toOutput[1]));
        }

        public Task ProvidePreference(ItemPreference itemPreference)
        {
            itemPreferences.Add(itemPreference);
            return Task.CompletedTask;
        }

        public Task InitialiseDatabase(List<string> values)
        {
            items = values;
            return Task.CompletedTask;
        }

        public bool InitialisationRequired()
        {
            return true;
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> TakePercentage<T>(this IEnumerable<T> source, double percentage)
        {
            var numberToTake = (int)Math.Floor(source.Count() * (percentage / 100));
            Console.WriteLine(numberToTake);
            return source.Take(numberToTake);
        }
    }
}
