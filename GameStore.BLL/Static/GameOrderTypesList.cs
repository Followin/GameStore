using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GameStore.Domain.Entities;

namespace GameStore.BLL.Static
{
    public static class GameOrderTypesList
    {
        private static IDictionary<String, Expression<Func<Game, object>>> _dictionary;

        static GameOrderTypesList()
        {
            _dictionary = new Dictionary<string, Expression<Func<Game, object>>>();
            _dictionary.Add("Most popular", game => game.UsersViewed.Count);
            _dictionary.Add("Most commented", game => game.Comments.Count);
            _dictionary.Add("By price", game => game.Price);
            _dictionary.Add("New", game => game.IncomeDate);
        }

        public static IEnumerable<String> GetOrderKeys()
        {
            return _dictionary.Keys.AsEnumerable();
        }


        public static Expression<Func<Game, object>> GetOrderExpression(String key)
        {
            return _dictionary[key];
        }
    }
}
