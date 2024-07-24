using BethanysPieShopOnline.Models;
using Microsoft.AspNetCore.Mvc;

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
            return View(_pieRepository.AllPies);
        }
    }
}
