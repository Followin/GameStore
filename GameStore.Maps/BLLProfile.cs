using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.DTO;
using GameStore.BLL.QueryResults;
using GameStore.Domain.Entities;

namespace GameStore.Maps
{
    public class BLLProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Game, GameQueryResult>();
            Mapper.CreateMap<Game, GameDTO>();
            Mapper.CreateMap<Publisher, PublisherQueryResult>();

            // Commands -> Entities
            Mapper.CreateMap<CreateGameCommand, Game>()
                  .ForMember(g => g.Genres, _ => _.Ignore())
                  .ForMember(g => g.PlatformTypes, _ => _.Ignore());

            Mapper.CreateMap<EditGameCommand, Game>()
                  .ForMember(g => g.Genres, _ => _.Ignore())
                  .ForMember(g => g.PlatformTypes, _ => _.Ignore());

            Mapper.CreateMap<CreateCommentCommand, Comment>()
                  .ForMember(x => x.GameId, _ => _.Ignore())
                  .ForMember(x => x.ParentCommentId, _ => _.Ignore());

            Mapper.CreateMap<CreatePublisherCommand, Publisher>()
                  .ForMember(x => x.Games, _ => _.Ignore());

            Mapper.CreateMap<CreateOrderDetailsCommand, OrderDetails>();

            // Entities -> DTOs
            Mapper.CreateMap<Comment, CommentDTO>();
            Mapper.CreateMap<Genre, GenreDTO>();
            Mapper.CreateMap<PlatformType, PlatformTypeDTO>();
            Mapper.CreateMap<Publisher, PublisherDTO>();
            Mapper.CreateMap<Game, GameDTO>();
        }
    }
}
