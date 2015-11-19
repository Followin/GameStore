using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Configurations
{
    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            Property(x => x.Name).IsRequired()
                                 .HasMaxLength(30);
            Property(x => x.Body).IsRequired();
        }
    }
}