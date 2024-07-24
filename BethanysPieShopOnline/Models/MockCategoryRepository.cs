namespace BethanysPieShopOnline.Models
{
    public class MockCategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> AllCategories =>
            new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "Fruit Pies", Description = "All-fruity pies." },
                new Category { CategoryId = 2, CategoryName = "Meat Pies", Description = "The suffering is inherent!" },
                new Category { CategoryId = 3, CategoryName = "Seasonal Pies", Description = "In the mood for something seasonal?" }
            };
    }
}
