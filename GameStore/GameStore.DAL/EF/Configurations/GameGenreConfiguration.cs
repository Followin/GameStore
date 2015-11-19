using System.Data.Entity.ModelConfiguration;
using GameStore.Domain.Entities;

namespace GameStore.DAL.EF.Configurations
{
    public class GameGenreConfiguration : EntityTypeConfiguration<GameGenre>
    {
        public GameGenreConfiguration()
        {
            HasKey(x => new {x.GenreId, x.GameId});
        }
    }
}