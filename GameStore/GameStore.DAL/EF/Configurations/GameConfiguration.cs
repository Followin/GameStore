using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Configurations
{
    public class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Key).IsRequired()
                                .HasMaxLength(50);
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(50);
            Property(x => x.DescriptionRu).IsRequired();
            Property(x => x.Discontinued).IsRequired()
                                         .HasColumnType("BIT");
            Property(x => x.UnitsInStock).IsRequired()
                                         .HasColumnType("SMALLINT");
            Property(x => x.Price).IsRequired()
                                  .HasColumnType("MONEY");
        }
    }
}