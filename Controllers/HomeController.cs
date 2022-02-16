﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using SheetCutting.Models;
using SheetCutting.Models.ViewModels;
using SheetCutting.Services;

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

        public IActionResult Index(IndexViewModel indexViewModel)
        {
            // Mock Data ////////////////////////////////////////////////////////////
            SheetViewModel sheet = new SheetViewModel()
            {
                Width = 1000,
                Height = 500
            };

            DetailInfoViewModel detail1 = new DetailInfoViewModel()
            {
                Width = 150,
                Height = 60, //50,
                BackgroundColor = BackgroundColor.blue,
                Count = 5
            };

            DetailInfoViewModel detail2 = new DetailInfoViewModel()
            {
                Width = 93,
                Height = 70,
                BackgroundColor = BackgroundColor.yellow,
                Count = 5
            };

            DetailInfoViewModel detail3 = new DetailInfoViewModel()
            {
                Width = 400,
                Height = 80,
                BackgroundColor = BackgroundColor.red,
                Count = 5 // 6 // 7
            };

            List<DetailInfoViewModel> detailsInfo = new List<DetailInfoViewModel>()
            {
                detail1, detail2, detail3
            };
            // ///////////////////////////////////////////////////////////////////////

            List<DetailViewModel> cutedDetails = _cuttingFormationService.Cut(sheet, detailsInfo);

            IndexViewModel viewModel = new IndexViewModel()
            {
                Sheet = sheet,
                DetailsInfo = detailsInfo,
                CuttedDetails = cutedDetails
            };

            return View(viewModel);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}