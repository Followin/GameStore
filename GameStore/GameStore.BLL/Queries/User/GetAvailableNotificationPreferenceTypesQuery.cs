using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.DAL.Abstract;

namespace GameStore.BLL.Queries.User
{
    public class GetAvailableNotificationPreferenceTypesQuery : IQuery
    {
        public int UserId { get; set; }
    }
}
