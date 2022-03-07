using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using SheetCutting.Models;
using SheetCutting.Models.ViewModels;
using SheetCutting.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SheetCutting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICuttingFormationService _cuttingFormationService;

        public HomeController(ILogger<HomeController> logger, ICuttingFormationService cuttingFormationService)
        {
            _logger = logger;
            _cuttingFormationService = cuttingFormationService;
        }

        public IActionResult Index()
        {
            IndexViewModel viewModel = new IndexViewModel()
            {
                Sheet = new(),
                DetailsInfo = new() { new DetailInfoViewModel() },
                //CuttedDetails = new() { new DetailViewModel() }
            };

            return View(viewModel);
        }

        public IActionResult FetchDetailsPartial([FromBody] IndexViewModel viewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    string failedChanges = "";

            //    foreach (ModelError error in ViewData.ModelState.Values.SelectMany(v => v.Errors))
            //    {
            //        failedChanges += error.ErrorMessage + "\n";
            //    }
            //    failedChanges = failedChanges.TrimEnd('\n');
            //    ViewBag.failedChanges = failedChanges.Split("\n");

            //    return View("_ErrorDetailsPartial", failedChanges.Split('\n'));
            //}

            try
            {
                viewModel.CuttedDetails = _cuttingFormationService.Cut(viewModel.Sheet, viewModel.DetailsInfo);
            }
            catch (Exception ex)
            {
                return PartialView("_ErrorDetailsPartial", ex.Message.TrimEnd().Split('\n'));
            }

            return PartialView("_DetailsPartial", viewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}