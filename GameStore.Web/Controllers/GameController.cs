using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Comment;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Comment;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults;
using GameStore.BLL.QueryResults.Comment;
using GameStore.BLL.QueryResults.Game;
using GameStore.Web.Models;
using GameStore.Web.Models.Comment;
using GameStore.Web.Models.Game;
using GameStore.Web.Models.Order;
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
            return View(game);
        }

        [HttpPost]
        [ActionName("NewComment")]
        public ActionResult CreateComment(String gamekey, CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CreateModel.ParentCommentId == null)
                {
                    var query = QueryDispatcher.Dispatch<GetGameByKeyQuery, GameQueryResult>(
                        new GetGameByKeyQuery
                        {
                            Key = gamekey
                        });
                    model.CreateModel.GameId = query.Id;
                }
                var command = Mapper.Map<CreateCommentViewModel, CreateCommentCommand>(model.CreateModel);
                CommandDispatcher.Dispatch(command);
                return RedirectToRoute(new { action = "Comments", controller = "Game", gamekey = gamekey });
            }

            var commentsQuery = QueryDispatcher.Dispatch<GetCommentsByGameKeyQuery, CommentsQueryResult>(
                new GetCommentsByGameKeyQuery
                {
                    Key = gamekey
                });
            var comments = Mapper.Map<IEnumerable<DisplayCommentViewModel>>(commentsQuery);
            model.Comments = comments;
            return View("Comments", model);
        }

        [HttpPost]
        public ActionResult DeleteComment(String gamekey, Int32 id)
        {
            var command = new DeleteCommentCommand { Id = id };
            CommandDispatcher.Dispatch(command);
            return RedirectToAction("Comments", "Game", new { gamekey });
        }

        public ActionResult Comments(String gamekey)
        {
            var query = QueryDispatcher.Dispatch<GetCommentsByGameKeyQuery, CommentsQueryResult>(
                new GetCommentsByGameKeyQuery
                {
                    Key = gamekey
                });
            var comments = Mapper.Map<IEnumerable<DisplayCommentViewModel>>(query);
            var model = new CommentViewModel { Comments = comments };
            return View(model);
        }

        public ActionResult Download(String gamekey)
        {
            return new FileContentResult(new byte[0], "application/pdf");
        }

        public ActionResult Buy(String gamekey, OrderViewModel currentOrder)
        {
            var game = QueryDispatcher.Dispatch<GetGameByKeyQuery, GameQueryResult>(
                new GetGameByKeyQuery { Key = gamekey });
            if (game == null)
            {
                return HttpNotFound();
            }

            var existingOrderDetail = currentOrder.OrderDetails.FirstOrDefault(x => x.GameId == game.Id);
            if (existingOrderDetail == null)
            {
                currentOrder.OrderDetails.Add(new OrderDetailsViewModel
                {
                    Discount = 0F,
                    GameId = game.Id,
                    GameName = game.Name,
                    Price = game.Price,
                    Quantity = 1
                });
                return RedirectToAction("Index", "Games");
            }
            existingOrderDetail.Quantity++;
            return RedirectToAction("Index", "Games");

        }
    }
}