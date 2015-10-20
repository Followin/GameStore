using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.DTO;
using GameStore.Web.Models;

namespace GameStore.Web.Utils
{
    public class WebMapperProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<CreateGameViewModel, CreateGameCommand>();
            Mapper.CreateMap<EditGameViewModel, EditGameCommand>();
            Mapper.CreateMap<CreateCommentViewModel, CreateCommentCommand>();

            Mapper.CreateMap<PlatformTypeDTO, PlatformTypeViewModel>();
            Mapper.CreateMap<GenreDTO, GenreViewModel>();
            Mapper.CreateMap<GameDTO, DisplayGameViewModel>();
            Mapper.CreateMap<CommentDTO, DisplayCommentViewModel>();
        }
    }
}