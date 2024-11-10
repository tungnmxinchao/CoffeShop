
using CoffeShop.Models;
using CoffeShop.Service;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorPages();
builder.Services.AddDbContext<CoffeShopContext>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});


builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<TableService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IngredientService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<OrderDetailsService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ReportService>();



var app = builder.Build();

app.UseSession();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/", async context =>
{
	context.Response.Redirect("/CoffeApp/Home");
});

app.Run();
