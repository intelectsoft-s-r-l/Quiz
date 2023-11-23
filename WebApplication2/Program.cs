using ISAdminWeb.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApplication2.Interface;
using WebApplication2.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();

builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                string relativeRedirectUri = new Uri(context.RedirectUri).PathAndQuery;

                if (Utils.IsAjaxRequest(context.Request))
                {
                    context.Response.Headers["Location"] = relativeRedirectUri;
                    context.Response.StatusCode = 401;
                }
                else
                {
                    context.Response.Redirect(relativeRedirectUri);
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


/*

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");
*/

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    //endpoints.MapControllerRoute(
    //   name: "License",
    //   pattern: "{controller=Home}/{action=Index}/{oid?}");

});
app.Run();
