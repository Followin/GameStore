using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Static;

namespace GameStore.BLL.DTO
{
    public abstract class EntityDTO<T>
    {
        protected EntityDTO()
        {
            EntryState = EntryState.Active;
        }

        public T Id { get; set; }

        public EntryState EntryState { get; set; }
    }
}
