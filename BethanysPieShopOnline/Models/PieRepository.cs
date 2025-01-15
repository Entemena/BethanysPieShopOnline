using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BethanysPieShopOnline.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly BethanysPieShopDbContext _dbcontext;

        public PieRepository(BethanysPieShopDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IEnumerable<Pie> AllPies
        {
            get
            {
                return _dbcontext.Pies.Include(c => c.Category);
            }
        }
        public IEnumerable<Pie> PiesOfTheWeek
        {
            get
            {
                return _dbcontext.Pies.Include(c => c.Category).Where(p => p.IsPieOfTheWeek);
            }
        }

        public Pie? GetPieById(int pieId)
        {
            return _dbcontext.Pies.FirstOrDefault(p => p.PieId == pieId);
        }

        public IEnumerable<Pie> SearchPies(string SearchQuery)
        {
            return _dbcontext.Pies.Where(p => p.Name.Contains(SearchQuery));
        }
    }
}
