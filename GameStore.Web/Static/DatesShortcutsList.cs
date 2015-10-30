using System;
using System.Collections.Generic;


namespace GameStore.Web.Static
{
    public static class DatesShortcutsList
    {
        private static Dictionary<String, TimeSpan> _dictionary;

        static DatesShortcutsList()
        {
            _dictionary = new Dictionary<String, TimeSpan>();
            _dictionary.Add("Last week", TimeSpan.FromDays(7));
            _dictionary.Add("Last month", TimeSpan.FromDays(31));
            _dictionary.Add("Last year", TimeSpan.FromDays(365));
            _dictionary.Add("2 Years", TimeSpan.FromDays(365 * 2));
            _dictionary.Add("3 Years", TimeSpan.FromDays(365*3));
        }

        public static DateTime? GetDate(String str)
        {
            if (str == null || !_dictionary.ContainsKey(str))
            {
                return null;
            }

            return DateTime.UtcNow - _dictionary[str];
        }

        public static IEnumerable<String> GetShortcuts()
        {
            return _dictionary.Keys;
        }
    }
}