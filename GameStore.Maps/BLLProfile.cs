using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Comment;
using GameStore.BLL.Commands.Game;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.Commands.Publisher;
using GameStore.BLL.Commands.User;
using GameStore.BLL.DTO;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.QueryResults.Order;
using GameStore.BLL.QueryResults.Publisher;
using GameStore.BLL.QueryResults.User;
using GameStore.Domain.Entities;

namespace GameStore.Maps
{
    public class BLLProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Game, GameQueryResult>();
            Mapper.CreateMap<GameDTO, GameQueryResult>();
            Mapper.CreateMap<Game, GameDTO>();
            Mapper.CreateMap<OrderDetails, OrderDetailsDTO>();

            Mapper.CreateMap<Publisher, PublisherQueryResult>();
            Mapper.CreateMap<User, UserQueryResult>();
            Mapper.CreateMap<Order, OrderQueryResult>();

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

            Mapper.CreateMap<CreateUserCommand, User>();
                  //.ForMember(x => x.Orders, _ => _.Ignore());

            Mapper.CreateMap<CreateOrderDetailsCommand, OrderDetails>();

            // Entities -> DTOs
            Mapper.CreateMap<Comment, CommentDTO>();
            Mapper.CreateMap<Genre, GenreDTO>();
            Mapper.CreateMap<PlatformType, PlatformTypeDTO>();
            Mapper.CreateMap<Publisher, PublisherDTO>();
            Mapper.CreateMap<Game, GameDTO>();
            Mapper.CreateMap<Shipper, ShipperDTO>();
        }
    }
}
