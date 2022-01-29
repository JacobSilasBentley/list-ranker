using System;
using System.Collections.Generic;
using System.Linq;

namespace ListRanker.Application
{
    public static class RankingCalculator
    {
        public static IReadOnlyList<RankedListItem> CalculateRank(IEnumerable<ItemPreference> itemPreferences)
        {
            List<RankedListItem> rankedListItems = itemPreferences
                .SelectMany(i => new List<ListItem>() { i.Item1, i.Item2 })
                .GroupBy(i => i.ID)
                .Select(i => i.First())
                .Select(i => new RankedListItem(i))
                .ToList();

            foreach(var preference in itemPreferences)
            {
                RankedListItem ranked1 = rankedListItems.First(i => i.ID == preference.Item1.ID);
                RankedListItem ranked2 = rankedListItems.First(i => i.ID == preference.Item2.ID);

                var newItem1Rating = calculateUpdatedRating(ranked1.Rating, ranked2.Rating, preference, ItemPreference.PairPreference.PreferItem1);
                var newItem2Rating = calculateUpdatedRating(ranked2.Rating, ranked1.Rating, preference, ItemPreference.PairPreference.PreferItem2);

                ranked1.Rating = newItem1Rating;
                ranked2.Rating = newItem2Rating;
            }

            return RankedListItem.FindPositionForRankedItems(rankedListItems).OrderBy(i => i.Position).ToList();
        }

        private static double calculateUpdatedRating(double yourRating, double opponentRating, ItemPreference preference, ItemPreference.PairPreference winningPreference)
        {
            double expectedOutcome = getExpectedOutcome(yourRating, opponentRating);
            Console.WriteLine($"Expected outcome: {expectedOutcome}");
            int score = preference.Preference == winningPreference ? 1 : 0;
            return getUpdatedRating(yourRating, score, expectedOutcome);
        }

        private static double getExpectedOutcome(double ra, double rb)
        {
            Console.WriteLine($"ra: {ra}, rb:{rb}");
            var factor = (rb - ra)/400;
            return 1 / (1 + Math.Pow(10, factor));
        }

        private static double getUpdatedRating(double rating, double score, double expectedScore)
        {
            double factor = 32;
            return rating + factor * (score - expectedScore);
        }
    }
}
