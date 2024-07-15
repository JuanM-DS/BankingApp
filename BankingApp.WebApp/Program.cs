using BankingApp.Infrastructure.Identity;
using BankingApp.Infrastructure.Persistence;
using BankingApp.Infrastructure.Shared;
using BankingApp.Core.Application;
using BankingApp.WebApp.Services;
using BankingApp.WebApp.Middlewares;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


#region Services
builder.Services.AddSession();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<ImageServices, ImageServices>();
builder.Services.AddScoped<LoginAuthorize>();
#endregion


var app = builder.Build();

await app.RunIdentitySeeds();
await app.RunPersistenceSeeds();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Login/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
