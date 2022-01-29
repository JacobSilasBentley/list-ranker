using System;

namespace ListRanker.Application
{
    public class ListItem
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ListItem(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            Name = name;
        }

        public ListItem()
        {}

        public override string ToString() => Name;
    }
}
