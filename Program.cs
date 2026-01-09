using csharp_groep31.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using csharp_groep31.Services.Interfaces;
using csharp_groep31.Services;


var builder = WebApplication.CreateBuilder(args);

// Voor Swagger documentatie
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Services registreren
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IEnclosureService, EnclosureService>();
builder.Services.AddScoped<IZooService, ZooService>();

// DbContext registreren
builder.Services.AddDbContext<ZooContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ZooDb")
    )
);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Dit is nodig voor de API zodat hij niet in een ondeindige loop komt bij ophalen
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Hiermee wordt de database automatisch gemigreerd zodat we niet steeds in console hoeven te migreren
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ZooContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection(); // We gebruiken HTTPS i.p.v. HTTP
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Voor de API
app.MapControllers();

app.Run();
