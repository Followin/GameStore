using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Save all maden changes
        /// </summary>
        void Save();
    }
}
