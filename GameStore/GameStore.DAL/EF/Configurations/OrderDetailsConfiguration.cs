using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Configurations
{
    public class OrderDetailsConfiguration : EntityTypeConfiguration<OrderDetails>
    {
        public OrderDetailsConfiguration()
        {
            ToTable("OrderDetails");

            HasKey(x => new {x.GameId, x.OrderId});
            Property(x => x.Quantity).IsRequired()
                                     .HasColumnType("SMALLINT");
            Property(x => x.Discount).HasColumnType("REAL");
            Property(x => x.Price).IsRequired()
                                  .HasColumnType("MONEY");
        }
    }
}