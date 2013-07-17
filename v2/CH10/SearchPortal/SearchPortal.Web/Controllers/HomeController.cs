using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SearchPortal.Web.Framework.Contracts;
using SearchPortal.Web.ViewModels;

namespace SearchPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnumerable<ISearchEngine> _searchEngines;

        public HomeController()
        {
            _searchEngines = MvcApplication.Container.GetExportedValues<ISearchEngine>();
        }

        public ActionResult Index()
        {
            if (!_searchEngines.Any())
            {
                return RedirectToAction("NoSearchEnginesInstalled");
            }

            var viewModel = new SearchFormViewModel();
            viewModel.SearchTerm = "";
            viewModel.SearchEngine = _searchEngines.Select(e => e.Name).FirstOrDefault();
            viewModel.AvailableSearchEngines = _searchEngines.Select(
                e => new SelectListItem()
                {
                    Text = e.Name,
                    Value = e.Name
                }).ToList();

            return View(viewModel);
        }

        [HttpPost, ActionName("Index")]
        public ActionResult Index_Post(SearchFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var searchEngine = _searchEngines.FirstOrDefault(e => e.Name == model.SearchEngine);
                if (searchEngine != null)
                {
                    return Redirect(searchEngine.GenerateSearchUri(model.SearchTerm).ToString());
                }
            }

            return View(model);
        }

        public ActionResult NoSearchEnginesInstalled()
        {
            return View();
        }
    }
}
