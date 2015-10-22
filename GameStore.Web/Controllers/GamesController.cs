using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.Queries.Genre;
using GameStore.BLL.Queries.PlatformType;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryResults;
using GameStore.Web.Models;
using GameStore.Web.Models.Game;
using NLog;

namespace GameStore.Web.Controllers
{
    public class GamesController : BaseController
    {
        public GamesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger)
            : base(commandDispatcher, queryDispatcher, logger)
        {
        }

        public ActionResult Index()
        {
            var gamesQuery = QueryDispatcher.Dispatch<GetAllGamesQuery, GamesQueryResult>(new GetAllGamesQuery());
            var gamesVM = Mapper.Map<IEnumerable<DisplayGameViewModel>>(gamesQuery);
            return Json(gamesVM, JsonRequestBehavior.AllowGet);
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

        [ChildActionOnly]
        [OutputCache(Duration = 60)]
        public ActionResult GetGamesCount()
        {
            var query = new GetGamesCountQuery();
            var queryResult = QueryDispatcher.Dispatch<GetGamesCountQuery, GamesCountQueryResult>(query);
            return PartialView("_gamesCount", queryResult.Count);
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
    }
}
