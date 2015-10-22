using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Comment;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults;
using GameStore.Web.Models;
using NLog;

namespace GameStore.Web.Controllers
{
    public class GameController : BaseController
    {
        public GameController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger)
            : base(commandDispatcher, queryDispatcher, logger)
        {
        }

        public ActionResult Details(String gamekey)
        {
            var query = QueryDispatcher.Dispatch<GetGameByKeyQuery, GameQueryResult>(
                new GetGameByKeyQuery
                {
                    Key = gamekey
                });
            var game = Mapper.Map<DisplayGameViewModel>(query);
            return Json(game, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateComment(String gamekey, CreateCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = Mapper.Map<CreateCommentViewModel, CreateCommentCommand>(model);
                CommandDispatcher.Dispatch(command);
            }

            return RedirectToAction("Index", "Games");
        }

        [HttpPost]
        public ActionResult CreateComment(CreateCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = Mapper.Map<CreateCommentViewModel, CreateCommentCommand>(model);
                CommandDispatcher.Dispatch(command);
            }

            return RedirectToAction("Index", "Games");
        }

        public ActionResult Comments(String gamekey)
        {
            var query = QueryDispatcher.Dispatch<GetCommentsByGameKeyQuery, CommentsQueryResult>(
                new GetCommentsByGameKeyQuery
                {
                    Key = gamekey
                });
            var comments = Mapper.Map<IEnumerable<DisplayCommentViewModel>>(query);
            return Json(comments, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Download(String gamekey)
        {
            return new FileContentResult(new byte[0], "application/pdf");
        }
    }
}