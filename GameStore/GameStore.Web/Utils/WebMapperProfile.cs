using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using AutoMapper;
using GameStore.Auth.Models;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Comment;
using GameStore.BLL.Commands.Game;
using GameStore.BLL.Commands.Publisher;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults;
using GameStore.BLL.QueryResults.Comment;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.QueryResults.Genre;
using GameStore.BLL.QueryResults.Order;
using GameStore.BLL.QueryResults.Publisher;
using GameStore.Static;
using GameStore.Web.Models;
using GameStore.Web.Models.Account;
using GameStore.Web.Models.Comment;
using GameStore.Web.Models.Game;
using GameStore.Web.Models.Genres;
using GameStore.Web.Models.Order;
using GameStore.Web.Models.Publisher;
using GameStore.Web.Static;

namespace GameStore.Web.Utils
{
    public class WebMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<CreateGameModel, CreateGameCommand>();
            Mapper.CreateMap<EditGameViewModel, EditGameCommand>();
            Mapper.CreateMap<CreateCommentViewModel, CreateCommentCommand>();
            Mapper.CreateMap<CreatePublisherViewModel, CreatePublisherCommand>();

            Mapper.CreateMap<PlatformTypeDTO, PlatformTypeViewModel>();
            Mapper.CreateMap<GenreDTO, GenreViewModel>()
                .ForMember(x => x.Name, _ => _.ResolveUsing(GetGenreName));
            Mapper.CreateMap<GenreQueryResult, GenreViewModel>()
                  .ForMember(x => x.Name, _ => _.ResolveUsing(GetGenreName));
            Mapper.CreateMap<PublisherDTO, DisplayPublisherViewModel>();
            Mapper.CreateMap<GameDTO, DisplayGameModel>()
                .ForMember(x => x.Description, _ => _.ResolveUsing(GetGameDescription))
                .ForMember(x => x.IsDeleted, _ => _.MapFrom(x => x.EntryState == EntryState.Deleted));
            Mapper.CreateMap<CommentDTO, DisplayCommentViewModel>();
            Mapper.CreateMap<OrderDetailsDTO, OrderDetailsViewModel>();
            Mapper.CreateMap<OrderQueryResult, OrderViewModel>();
            Mapper.CreateMap<ShipperDTO, ShipperViewModel>();


            Mapper.CreateMap<PublisherQueryResult, DisplayPublisherViewModel>();
            Mapper.CreateMap<CommentQueryResult, DisplayCommentViewModel>();
            Mapper.CreateMap<GameQueryResult, DisplayGameModel>()
                  .ForMember(x => x.IsDeleted, _ => _.MapFrom(x => x.EntryState == EntryState.Deleted));
            Mapper.CreateMap<GameFiltersModel, GetGamesQuery>()
                  .ForMember(x => x.Skip, _ => _.MapFrom(x => x.ItemsPerPage * (x.Page - 1)))
                  .ForMember(x => x.Number, _ => _.MapFrom(x => x.ItemsPerPage))
                  .ForMember(x => x.MinDate, _ => _.MapFrom(x => x.MinDateShortcut == DaysShortcut.Choose ? null : (DateTime?)DateTime.Now.AddDays(-1 * (Int32)x.MinDateShortcut)));

            Mapper.CreateMap<RegisterAccountViewModel, RegisterUserModel>()
                  .ForMember(x => x.Password, _ => _.MapFrom(x => x.Password))
                  .ForMember(x => x.Claims, _ => _.UseValue(new[] { new Claim(ClaimTypes.Role, Roles.User) }));

            Mapper.CreateMap<CreateUserViewModel, RegisterUserModel>()
                  .ForMember(x => x.Password, _ => _.MapFrom(x => x.Password))
                  .ForMember(x => x.Claims, _ => _.MapFrom(x =>
                      new[]
                      {
                          new Claim(ClaimTypes.Role, x.Role), 
                          new Claim(ClaimTypesExtensions.Publisher, x.PublisherId.ToString()),
                      }));

        }

        public String GetGameDescription(GameDTO gameDto)
        {
            switch (Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2))
            {
                case "ru":
                    return gameDto.DescriptionRu;
                case "en":
                    return gameDto.DescriptionEn;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public String GetGenreName(GenreDTO genreDto)
        {
            switch (Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2))
            {
                case "ru":
                    return genreDto.NameRu;
                case "en":
                    return genreDto.NameEn;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public String GetGenreName(GenreQueryResult genreDto)
        {
            switch (Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2))
            {
                case "ru":
                    return genreDto.NameRu;
                case "en":
                    return genreDto.NameEn;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}