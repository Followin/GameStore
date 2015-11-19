using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Configurations
{
    public class PublisherConfiguration : EntityTypeConfiguration<Publisher>
    {
        public PublisherConfiguration()
        {
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CompanyName).IsRequired()
                                        .HasMaxLength(40);
            Property(x => x.HomePage).IsRequired()
                                     .HasColumnType("NTEXT");
            Property(x => x.Description).IsRequired()
                                        .HasColumnType("NTEXt");
        }
    }
}