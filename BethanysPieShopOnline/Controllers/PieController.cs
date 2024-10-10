using BethanysPieShopOnline.Models;
using Microsoft.AspNetCore.Mvc;
using BethanysPieShopOnline.ViewModels;

namespace BethanysPieShopOnline.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _CategoryRepository;

        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository)
        {
            _pieRepository = pieRepository;
            _CategoryRepository = categoryRepository;
        }

        public IActionResult List() 
        {
            PieListViewModel pieListViewModel = new PieListViewModel
                (_pieRepository.AllPies, "Cheesecakes");
            return View(pieListViewModel);
        }
        public IActionResult Details(int id) 
        { 
            var pie = _pieRepository.GetPieById(id);
            if (pie == null) { return NotFound(); }
            return View(pie);
        }
    }
}
