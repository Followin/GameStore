using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Comment;
using GameStore.BLL.Commands.Game;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Comment;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.Queries.Genre;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.Queries.PlatformType;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryResults;
using GameStore.BLL.QueryResults.Comment;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.QueryResults.Genre;
using GameStore.BLL.QueryResults.Order;
using GameStore.BLL.QueryResults.PlatformType;
using GameStore.BLL.QueryResults.Publisher;
using GameStore.Web.Abstract;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Models;
using GameStore.Web.Models.Comment;
using GameStore.Web.Models.Game;
using GameStore.Web.Models.Genres;
using GameStore.Web.Models.Order;
using GameStore.Web.Models.Publisher;
using GameStore.Web.Static;
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

            var game = Mapper.Map<DisplayGameModel>(query);
            return View(game);
        }

        public ActionResult Comments(String gamekey)
        {
            var query = QueryDispatcher.Dispatch<GetCommentsByGameKeyQuery, CommentsQueryResult>(
                new GetCommentsByGameKeyQuery
                {
                    Key = gamekey
                });
            var comments = Mapper.Map<IEnumerable<DisplayCommentViewModel>>(query);
            var model = new CommentViewModel { Comments = comments, GameId = query.GameId, IsDeleted = query.GameIsDeleted };
            return View(model);
        }

        public ActionResult Download(String gamekey)
        {
            return new FileContentResult(new byte[0], "application/pdf");
        }

        public ActionResult Buy(String gamekey)
        {
            var game = QueryDispatcher.Dispatch<GetGameByKeyQuery, GameQueryResult>(
                new GetGameByKeyQuery { Key = gamekey });
            if (game == null)
            {
                return HttpNotFound();
            }

            var currentOrder = QueryDispatcher.Dispatch<GetCurrentOrder, OrderQueryResult>(new GetCurrentOrder {UserId = 1});

            var newOrderDetails = new CreateOrderDetailsCommand
            {
                Discount = 0F,
                GameId = game.Id,
                OrderId = currentOrder.Id,
                Price = game.Price,
                Quantity = 1
            };

            CommandDispatcher.Dispatch(newOrderDetails);


            return RedirectToAction("Index", "Game");

        }

        public ActionResult Index([Bind(Prefix = "filterModel")] GameFiltersModel model)
        {
            var displayViewModel = new DisplayGameViewModel();
            var query = new GetGamesQuery();

            if (model == null)
            {
                model = new GameFiltersModel();
            }


            if (model.Page == 0)
            {
                model.Page = 1;
            }


            Mapper.Map(model, query);

            var queryResult = QueryDispatcher.Dispatch<GetGamesQuery, GamesPartQueryResult>(query);
            var viewModel = new PagedDisplayGameModel
            {
                DisplayModel = Mapper.Map<GamesPartQueryResult, IEnumerable<DisplayGameModel>>(queryResult),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = model.ItemsPerPage ?? Int32.MaxValue,
                    CurrentPage = model.Page,
                    TotalItems = queryResult.Count
                }
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("_gamesList", viewModel);
            }

            displayViewModel.Model = viewModel;
            FillFilterLists(displayViewModel);
            return View(displayViewModel);
        }


        
        [HttpGet]
        [ActionName("New")]
        [Authorize(Roles="Manager")]
        public ActionResult Create()
        {
            var model = new CreateGameViewModel();
            FillCreateGameViewModel(model);
            return View("Create", model);
        }

        [HttpPost]
        [ActionName("New")]
        [Authorize(Roles = "Manager")]
        public ActionResult Create(CreateGameViewModel model)
        {
            if (ModelState.IsValid)
            {
                CommandDispatcher.Dispatch(Mapper.Map<CreateGameModel, CreateGameCommand>(model.CreateModel));
                return RedirectToAction("Index");
            }

            FillCreateGameViewModel(model);
            return View("Create", model);
        }

        [HttpPost]
        [ActionName("Update")]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(EditGameViewModel model)
        {
            if (ModelState.IsValid)
            {
                CommandDispatcher.Dispatch(Mapper.Map<EditGameCommand>(model));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult Remove(String key)
        {
            CommandDispatcher.Dispatch(new DeleteGameCommand { Key = key });
            return RedirectToAction("Index");
        }


        [OutputCache(Duration = 60)]
        public ActionResult GetGamesCount()
        {
            var query = new GetGamesCountQuery();
            var queryResult = QueryDispatcher.Dispatch<GetGamesCountQuery, CountQueryResult>(query);
            return Json(queryResult.Count, JsonRequestBehavior.AllowGet);
        }

        private void FillCreateGameViewModel(CreateGameViewModel model)
        {
            var genres = Mapper.Map<GenresQueryResult, IEnumerable<GenreViewModel>>(
                QueryDispatcher.Dispatch<GetAllGenresQuery, GenresQueryResult>(new GetAllGenresQuery()));
            var platformTypes = Mapper.Map<PlatformTypesQueryResult, IEnumerable<PlatformTypeViewModel>>(
                QueryDispatcher.Dispatch<GetAllPlatformTypesQuery, PlatformTypesQueryResult>(
                    new GetAllPlatformTypesQuery()));
            var publishers =
                QueryDispatcher.Dispatch<GetAllPublishersQuery, PublishersQueryResult>(
                    new GetAllPublishersQuery());
            var publishersSelectList = new SelectList(publishers, "Id", "CompanyName");

            model.Genres = genres;
            model.PlatformTypes = platformTypes;
            model.Publishers = publishersSelectList;
        }

        private void FillFilterLists(DisplayGameViewModel model)
        {
            var genres = Mapper.Map<GenresQueryResult, IEnumerable<GenreViewModel>>(
                QueryDispatcher.Dispatch<GetAllGenresQuery, GenresQueryResult>(new GetAllGenresQuery()));

            var platformTypes = Mapper.Map<PlatformTypesQueryResult, IEnumerable<PlatformTypeViewModel>>(
                QueryDispatcher.Dispatch<GetAllPlatformTypesQuery, PlatformTypesQueryResult>(
                    new GetAllPlatformTypesQuery()));

            var publishers = Mapper.Map<PublishersQueryResult, IEnumerable<DisplayPublisherViewModel>>(
                QueryDispatcher.Dispatch<GetAllPublishersQuery, PublishersQueryResult>(
                    new GetAllPublishersQuery()));

            var itemsPerPageVariantsList = ItemsPerPageVariants.Variants.Select(x =>
                new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.ToString()
                }).ToList();
            itemsPerPageVariantsList.Insert(0, new SelectListItem
            {
                Text = GlobalRes.All,
                Value = ""
            });


            model.Publishers = publishers;
            model.Genres = genres;
            model.ItemsPerPageVariants = itemsPerPageVariantsList;
            model.PlatformTypes = platformTypes;

        }
    }
}