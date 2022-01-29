using System.Collections.Generic;
using System.Linq;

namespace ListRanker.Application
{
    public class RankedListItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public int Position { get; set; }

        public RankedListItem(ListItem itemPreference)
        {
            ID = itemPreference.ID;
            Name = itemPreference.Name;
            Rating = 1000;
        }

        public static IEnumerable<RankedListItem> FindPositionForRankedItems(IEnumerable<RankedListItem> input)
        {
            return input
                .GroupBy(i => i.Rating)
                .OrderByDescending(x => x.Key)
                .SelectMany((x, index) =>
                {
                    return x.Select(i =>
                    {
                        i.Position = index + 1;
                        return i;
                    });
                });
        }

        public override string ToString() => $"ID:{ID}, Name:{Name}, Rating:{Rating}";
    }
}
