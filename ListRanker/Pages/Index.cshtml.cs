using ListRanker.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListRanker.Pages
{
    public class IndexModel : PageModel
    {
        private static string cookieKey => "list-rank-identifier";

        [BindProperty]
        public ListItem ListItem1 { get; set; }
        [BindProperty]
        public ListItem ListItem2 { get; set; }
        [BindProperty]
        public string CookieValue { get; set; }


        private readonly ILogger<IndexModel> _logger;
        private readonly IDataStore _dataStore;

        public IndexModel(ILogger<IndexModel> logger, IDataStore dataStore)
        {
            _logger = logger;
            _dataStore = dataStore;
        }

        public async Task OnGet()
        {
            CookieValue = Request.Cookies[cookieKey];
            if(CookieValue is null)
            {
                CookieValue = Guid.NewGuid().ToString();
                Response.Cookies.Append("list-rank-identifier", CookieValue);
            }
            (ListItem1, ListItem2) = await _dataStore.GetTwoRandomItems();
            Console.WriteLine(ListItem1.Name);
        }

        public async Task<IActionResult> OnPostItem1()
        {
            await submitPreference(ItemPreference.PairPreference.PreferItem1);
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostItem2()
        {
            await submitPreference(ItemPreference.PairPreference.PreferItem2);
            return RedirectToPage("./Index");
        }

        private async Task submitPreference(ItemPreference.PairPreference item)
        {
            Console.WriteLine(ListItem1);
            ItemPreference preference = new ItemPreference()
            {
                UserIdentifier = CookieValue,
                Item1 = ListItem1, 
                Item2 = ListItem2,
                Preference = item
            };
            await _dataStore.ProvidePreference(preference);
        }
    }
}
