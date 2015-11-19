using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Configurations
{
    public class GenreConfiguration : EntityTypeConfiguration<Genre>
    {
        public GenreConfiguration()
        {
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.NameRu).IsRequired()
                                   .HasMaxLength(50);
            Property(x => x.NameEn).IsRequired()
                                   .HasMaxLength(50);
        }
    }
}