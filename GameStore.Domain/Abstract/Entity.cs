namespace GameStore.Domain.Abstract
{
    public enum EntryState
    {
        /// <summary>
        /// Active item
        /// </summary>
        Active,

        /// <summary>
        /// Temporary inactive item(disabled for search/login, etc...)
        /// </summary>
        Inactive,

        /// <summary>
        /// Deleted item
        /// </summary>
        Deleted
    }

    public abstract class Entity<T>
    {
        protected Entity()
        {
            EntryState = EntryState.Active;
        }

        public T Id { get; set; }

        public EntryState EntryState { get; set; }
    }
}
