using FluentValidation.AspNetCore;
using ISAdminWeb.Common;
using ISAdminWeb.Models;
using ISAdminWeb.Models.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using WebApplication2.Interface;
using WebApplication2.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();//.AddFluentValidation();
//builder.Services.AddTransient<IValidator<AuthRecoverpwViewModel>, AuthRecoverpwViewModelVolidator>();

//Added fluent validation
builder.Services.AddControllers().AddFluentValidation(options =>
{
    // Automatic registration of validators in assembly
    //options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    options.RegisterValidatorsFromAssemblyContaining<Program>();
    options.LocalizationEnabled = true;
});

ValidatorViewModel validatorViewModel = new ValidatorViewModel(builder.Services);

builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
// Add services to the container.
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services to the container.
builder.Services.AddMvc()
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization();



builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var supportedCultures = new[] { "en", "ro", "ru" };
    opt.SetDefaultCulture(supportedCultures[2])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});


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
var supportedCultures = new[] { "en", "ro", "ru" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[2])
.AddSupportedCultures(supportedCultures)
.AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);



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
