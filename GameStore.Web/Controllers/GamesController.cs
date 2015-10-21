using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries;
using GameStore.BLL.QueryResults;
using GameStore.Web.Models;
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
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(CreateGameViewModel model)
        {
            if (ModelState.IsValid)
            {
                CommandDispatcher.Dispatch(Mapper.Map<CreateGameCommand>(model));
            }

            return RedirectToAction("Index");
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
    }
}
