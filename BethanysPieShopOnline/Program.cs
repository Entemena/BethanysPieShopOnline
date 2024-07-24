using BethanysPieShopOnline.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
builder.Services.AddScoped<IPieRepository, MockPieRepository>();


/*
builder.Services.AddScoped          //Creates a singleton whilst the request is being handled.
builder.Services.AddTransient       // A new instance every time
builder.Services.AddSingleton       // Creates one instance that is kept alive
*/

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapDefaultControllerRoute();

app.Run();
