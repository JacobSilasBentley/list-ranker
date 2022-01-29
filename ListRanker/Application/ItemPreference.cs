namespace ListRanker.Application
{
    public class ItemPreference
    {
        public int ID { get; set; }
        public ListItem Item1 { get; set; }
        public ListItem Item2 { get; set; }
        public string UserIdentifier { get; set; }
        public PairPreference Preference { get; set; }

        public enum PairPreference
        {
            PreferItem1,
            PreferItem2,
        }

        public override string ToString() => $"{Item1} - {Item2} : {Preference} ({UserIdentifier})";
    }
}
