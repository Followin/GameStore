using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Abstract
{
    public interface ICommandRepository<in TEntity, in TKey>
    {
        void Add(TEntity item);

        void Update(TEntity item);

        void Delete(TKey id);
    }
}
