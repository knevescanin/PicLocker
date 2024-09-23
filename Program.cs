using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorApp.Areas.Identity;
using BlazorApp.Repositories;
using BlazorApp.Utilities;
using Microsoft.AspNetCore.SignalR;
using BlazorApp.Hubs;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BlazorAppIdentityDbContextConnection");
builder.Services.AddDbContext<BlazorAppIdentityDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<ApplicationUser>(
        options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        }
        )
        .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BlazorAppIdentityDbContext>();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
builder.Services.AddTransient<IImageUploadRepository,ImageUploadRepository>();
builder.Services.AddTransient<IFileUploadService,FileUploadService>();
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();

builder.Services.AddSignalR();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();

app.MapHub<ImageHub>("/imagehub");
app.MapHub<UserHub>("/userhub");
app.MapFallbackToPage("/_Host");


app.Run();