using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public abstract class EntityDTO<T>
    {
        public T Id { get; set; }
        public EntryState EntryState { get; set; }

    }

    public enum EntryState
    {
        Active,
        Inactive,
        Deleted
    }
}
