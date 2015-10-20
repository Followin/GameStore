namespace GameStore.Domain.Abstract
{
    public abstract class Entity <T>
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
