namespace BethanysPieShopOnline.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<PieListViewModel> PiesOfTheWeek { get; }

        public HomeViewModel(IEnumerable<PieListViewModel> piesOfTheWeek)
        {
            PiesOfTheWeek = piesOfTheWeek;
        }
    }
}
