var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("ReactHost",
            builder => builder
                .WithOrigins("https://localhost:44484")  // Update with your client's origin
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

var app = builder.Build();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromDays(10);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("ReactHost");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
