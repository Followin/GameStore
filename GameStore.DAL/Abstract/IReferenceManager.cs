using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.References;

namespace GameStore.DAL.Abstract
{
    public interface IReferenceManager<T>
    {
        IEnumerable<T> Sync(IEnumerable<T> items, DatabaseNames databaseName);

        T Sync(T item, DatabaseNames databaseName);

        Dictionary<DatabaseNames, IEnumerable<T>> Split(IEnumerable<T> items);

        DatabaseNames Split(T item);

        Reference GetReference(Int32 id);

        void CreateReference(Reference reference);

        void UpdateReference(Reference reference);

        void DeleteReference(Int32 id);
    }
}
