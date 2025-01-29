using BethanysPieShopOnline.Models;
using Microsoft.AspNetCore.Components;

namespace BethanysPieShopOnline.App.Pages
{
    public partial class PieCard
    {
        [Parameter]
        public Pie? Pie { get; set; }
    }
}
