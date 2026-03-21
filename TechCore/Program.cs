using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TechCore.Datos;
using TechCore.Services.Concretes.Dashboard;
using TechCore.Services.Concretes.Login;
using TechCore.Services.Concretes.Producto;
using TechCore.Services.Concretes.Usuario;
using TechCore.Services.Interfaces.Dashboard;
using TechCore.Services.Interfaces.Login;
using TechCore.Services.Interfaces.Producto;
using TechCore.Services.Interfaces.Usuario;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//---------------------SERVICES---------------------

builder.Services.AddDbContext<TechCoreContext>(options => {

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;


    if (connectionString.Contains("neon.tech") || connectionString.Contains("Host="))
    {
        options.UseNpgsql(connectionString);
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddScoped<ILogin, LoginService>();
builder.Services.AddScoped<IUsuario, UsuarioService>();
builder.Services.AddScoped<IBodegaDashboard, BodegaDashboardService>();
builder.Services.AddScoped<IProducto, ProductoService>();
builder.Services.AddHttpClient();

var app = builder.Build();

//-----------------MIDLEWARE-----------------

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
