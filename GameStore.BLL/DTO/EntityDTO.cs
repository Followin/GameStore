using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public enum EntryState
    {
        /// <summary>
        /// Active element
        /// </summary>
        Active,

        /// <summary>
        /// Temporary inactive element
        /// </summary>
        Inactive,

        /// <summary>
        /// Deleted element
        /// </summary>
        Deleted
    }

    public abstract class EntityDTO<T>
    {
        public T Id { get; set; }

        public EntryState EntryState { get; set; }
    }
}
