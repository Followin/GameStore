using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Utils
{
    public class FunctorComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> m_Comparison;

        public FunctorComparer(Comparison<T> Comparison)
        {
            m_Comparison = Comparison;
        }

        public int Compare(T x, T y)
        {
            return m_Comparison(x, y);
        }


    }

    public static class ComparisonCreator
    {
        public static Comparison<TKey> Compare<TKey, TRes>(Func<TKey, TRes> Selector) where TRes : IComparable<TRes>
        {
            return (x, y) => Selector(x).CompareTo(Selector(y));
        }
    }


}
