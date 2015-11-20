namespace GameStore.Static
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
}