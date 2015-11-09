using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.Utils;
using GameStore.Domain.Entities;

namespace GameStore.BLL.Static
{
    public static class GameOrderTypesList
    {
        private static IDictionary<String, String> _dictionary;

        static GameOrderTypesList()
        {
            _dictionary = new Dictionary<String, String>();
            _dictionary.Add("New", "incomeDate");
            _dictionary.Add("Most popular", "views");
            _dictionary.Add("Most commented", "comments");
            _dictionary.Add("By price ascending", "priceAsc");
            _dictionary.Add("By price descending", "priceDesc");
        }

        public static IEnumerable<String> GetOrderKeys()
        {
            return _dictionary.Keys.AsEnumerable();
        }


        public static String GetOrderExpression(String key)
        {
            return _dictionary[key];
        }
    }
}
