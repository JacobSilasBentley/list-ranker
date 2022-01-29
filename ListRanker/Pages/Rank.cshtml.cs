using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListRanker.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ListRanker.Pages
{
    public class RankModel : PageModel
    {
        public IReadOnlyList<RankedListItem> RankedList { get; set; }

        private readonly IDataStore _dataStore;

        public RankModel(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task OnGet()
        {
            List<ItemPreference> preferences = await _dataStore.GetItemPreferences();
            RankedList = RankingCalculator.CalculateRank(preferences);
        }
    }
}
