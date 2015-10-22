using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryResults;
using GameStore.Web.Models.Publisher;
using NLog;

namespace GameStore.Web.Controllers
{
    public class PublisherController : BaseController
    {
        public PublisherController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }

        public ActionResult Details(String companyName)
        {
            var query = QueryDispatcher.Dispatch<GetPublisherByCompanyNameQuery, PublisherQueryResult>(
                new GetPublisherByCompanyNameQuery { CompanyName = companyName });
            var model = Mapper.Map<PublisherQueryResult, DisplayPublisherViewModel>(query);

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreatePublisherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = Mapper.Map<CreatePublisherViewModel, CreatePublisherCommand>(model);
                CommandDispatcher.Dispatch(command);
                return RedirectToAction("Index", "Games");
            }

            return View(model);
        }
    }
}