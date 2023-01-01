using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options ;
using System.Reflection; 
using BlogApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddMvc()
  .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
         options.DataAnnotationLocalizerProvider = (type, factory) =>
                    { 
       var assemblyName = new AssemblyName(typeof(ChangeLang).GetTypeInfo().Assembly.FullName);

       return factory.Create("ChangeLang", assemblyName.Name);

      };
 });

builder.Services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                        {
                            new CultureInfo("en-US"),
                            new CultureInfo("tr-TR"),
                        };

                    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());

                });

builder.Services.AddDbContext<BlogApp.Models.MainContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "BlogApp";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        // giriş yaptıgında sure uzatma
        options.SlidingExpiration = false;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

//  var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
// var options2 = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
// var opt = app.Services.GetService <IOptions<RequestLocalizationOptions>>().Value;
var options = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();

app.UseRequestLocalization(options.Value);
//  app.UseRequestLocalization(opt);
app.UseStaticFiles();

app.UseRouting();
// bunu dahil etmezsen auth durumuna bakmaz cookiye 
app.UseAuthentication();
app.UseAuthorization();


// var options2 = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

// var opt = app.Services.GetService <IOptions<RequestLocalizationOptions>>().Value;

// app.UseRequestLocalization(opt);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
