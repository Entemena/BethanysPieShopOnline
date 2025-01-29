using BethanysPieShopOnline.Models;
using BethanysPieShopOnline.App;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

/*
builder.Services.AddScoped          //Creates a singleton whilst the request is being handled.
builder.Services.AddTransient       // A new instance every time
builder.Services.AddSingleton       // Creates one instance that is kept alive
*/

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddControllers();

builder.Services.AddDbContext<BethanysPieShopDbContext>(options => {
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:BethanysPieShopDbContextConnection"]);
});

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

app.UseAntiforgery();
app.MapRazorPages();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
//app.MapControllers();
app.UseStaticFiles();
app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Convention Based R
//app.MapDefaultControllerRoute();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

DbInitialiser.Seed(app);
app.Run();
