using GameStore.Static;

namespace GameStore.Domain.Entities
{
    public abstract class Entity<T>
    {
        protected Entity()
        {
            EntryState = EntryState.Active;
        }

        /// <summary>
        /// Primary key of an entity
        /// </summary>
        public T Id { get; set; }

        public EntryState EntryState { get; set; }
    }
}
