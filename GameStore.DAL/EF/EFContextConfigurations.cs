using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF
{
    public class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            Property(x => x.Key).IsRequired()
                                .HasMaxLength(50);
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(50);
            Property(x => x.Description).IsRequired();
            Property(x => x.Discontinued).IsRequired()
                                       .HasColumnType("BIT");
            Property(x => x.UnitsInStock).IsRequired()
                                         .HasColumnType("SMALLINT");
            Property(x => x.Price).IsRequired()
                                  .HasColumnType("MONEY");
            HasMany(x => x.UsersViewed).WithMany(x => x.ViewedGames);
        }
    }

    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(30);
            Property(x => x.Body).IsRequired();
        }
    }

    public class GenreConfiguration : EntityTypeConfiguration<Genre>
    {
        public GenreConfiguration()
        {
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(50);
        }
    }

    public class PlatformTypeConfiguration : EntityTypeConfiguration<PlatformType>
    {
        public PlatformTypeConfiguration()
        {
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(20);
        }
    }

    public class PublisherConfiguration : EntityTypeConfiguration<Publisher>
    {
        public PublisherConfiguration()
        {
            Property(x => x.CompanyName).IsRequired()
                                       .HasMaxLength(40);
            Property(x => x.HomePage).IsRequired()
                                     .HasColumnType("NTEXT");
            Property(x => x.Description).IsRequired()
                                        .HasColumnType("NTEXt");
        }
    }

    public class OrderDetailsConfiguration : EntityTypeConfiguration<OrderDetails>
    {
        public OrderDetailsConfiguration()
        {
            ToTable("OrderDetails");
            Property(x => x.Quantity).IsRequired()
                                     .HasColumnType("SMALLINT");
            Property(x => x.Discount).HasColumnType("REAL");
            Property(x => x.Price).IsRequired()
                                  .HasColumnType("MONEY");
        }
    }

    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            Property(x => x.Time).IsRequired();
        }
    }

    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(x => x.SessionId).IsRequired()
                                      .HasMaxLength(20);
        }
    }
}