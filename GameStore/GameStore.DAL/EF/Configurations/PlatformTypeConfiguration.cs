using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Configurations
{
    public class PlatformTypeConfiguration : EntityTypeConfiguration<PlatformType>
    {
        public PlatformTypeConfiguration()
        {
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(20);
        }
    }
}