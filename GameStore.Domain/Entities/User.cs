using System;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class User : Entity<Int32>
    {
        public String SessionId { get; set; }
    }
}
