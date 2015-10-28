using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Comment;
using GameStore.BLL.Commands.Game;
using GameStore.BLL.Commands.Publisher;
using GameStore.BLL.DTO;
using GameStore.BLL.QueryResults;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.QueryResults.Publisher;
using GameStore.Web.Models;
using GameStore.Web.Models.Comment;
using GameStore.Web.Models.Game;
using GameStore.Web.Models.Publisher;

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
            Mapper.CreateMap<GenreDTO, GenreViewModel>();
            Mapper.CreateMap<PublisherDTO, DisplayPublisherViewModel>();
            Mapper.CreateMap<GameDTO, DisplayGameModel>();
            Mapper.CreateMap<CommentDTO, DisplayCommentViewModel>();

            Mapper.CreateMap<PublisherQueryResult, DisplayPublisherViewModel>();
            Mapper.CreateMap<GameQueryResult, DisplayGameViewModel>();
        }
    }
}