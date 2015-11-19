using System;

namespace GameStore.DAL.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Save all maden changes
        /// </summary>
        void Save();
    }
}
