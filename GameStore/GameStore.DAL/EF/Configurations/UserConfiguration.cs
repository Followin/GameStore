using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
        }
    }
}