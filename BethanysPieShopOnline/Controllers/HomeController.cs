﻿using BethanysPieShopOnline.Models;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPieRepository _pieRepository;

        public HomeController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
