using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Game;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.Queries.Genre;
using GameStore.BLL.Queries.PlatformType;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryResults;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.QueryResults.Genre;
using GameStore.BLL.QueryResults.PlatformType;
using GameStore.BLL.QueryResults.Publisher;
using GameStore.BLL.Static;
using GameStore.Web.Models;
using GameStore.Web.Models.Game;
using GameStore.Web.Models.Publisher;
using GameStore.Web.Static;
using NLog;

namespace GameStore.Web.Controllers
{
    public class GamesController : BaseController
    {
        public GamesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger)
            : base(commandDispatcher, queryDispatcher, logger)
        {
        }

        public ActionResult Index([Bind(Prefix = "filterModel")] GameFiltersModel model)
        {
            var displayViewModel = new DisplayGameViewModel();
            var query = new GetGamesQuery();

            if (model == null)
            {
                model = new GameFiltersModel();
            }

            if (model.OrderBy == null)
            {
                model.OrderBy = "New";
            }

            if (model.Page == 0)
            {
                model.Page = 1;
            }

            if (model.ItemsPerPage == 0)
            {
                model.ItemsPerPage = 2;
            }

            Mapper.Map(model, query);
            
            var queryResult = QueryDispatcher.Dispatch<GetGamesQuery, GamesPartQueryResult>(query);
            var viewModel = new PagedDisplayGameModel
            {
                DisplayModel = Mapper.Map<GamesPartQueryResult, IEnumerable<DisplayGameModel>>(queryResult),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = model.ItemsPerPage,
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
        public ActionResult Create()
        {
            var model = new CreateGameViewModel();
            FillCreateGameViewModel(model);
            return View(model);
        }
        
        [HttpPost]
        public ActionResult Create(CreateGameViewModel model)
        {
            if (ModelState.IsValid)
            {
                CommandDispatcher.Dispatch(Mapper.Map<CreateGameModel, CreateGameCommand>(model.CreateModel));
                return RedirectToAction("Index");
            }

            FillCreateGameViewModel(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditGameViewModel model)
        {
            if (ModelState.IsValid)
            {
                CommandDispatcher.Dispatch(Mapper.Map<EditGameCommand>(model));
            }

            return RedirectToAction("Index");
        }
        
        [HttpPost]
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
            var publishersSelectList = new SelectList(publishers.Select(x =>
                new SelectListItem
                {
                    Text = x.CompanyName,
                    Value = x.Id.ToString()
                }));
            publishersSelectList = new SelectList(publishers, "Id", "CompanyName");

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

            model.Publishers = publishers;
            model.Genres = genres;
            model.OrderByVariants = GameOrderTypesList.GetOrderKeys().Select(x => 
                new SelectListItem
                {
                    Text = x,
                    Value = x
                });
            model.ItemsPerPageVariants = ItemsPerPageVariants.Variants.Select(x =>
                new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.ToString()
                });
            model.PlatformTypes = platformTypes;

        }
    }
}
