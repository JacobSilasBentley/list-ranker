using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListRanker.Application
{
    public interface IDataStore
    {
        Task InitialiseDatabase(List<string> values);
        Task<(ListItem, ListItem)> GetTwoRandomItems();
        Task ProvidePreference(ItemPreference itemPreference);
        Task<List<ItemPreference>> GetItemPreferences();
        bool InitialisationRequired();
    }
}
