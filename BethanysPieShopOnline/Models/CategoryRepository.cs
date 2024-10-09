namespace BethanysPieShopOnline.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BethanysPieShopDbContext _dbcontext;

        public CategoryRepository(BethanysPieShopDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IEnumerable<Category> AllCategories =>
            _dbcontext.Categories.OrderBy(p => p.CategoryName);
    }
}
