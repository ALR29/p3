using Microsoft.EntityFrameworkCore;
using GestorTareas.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuración de la Base de Datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=gestor_tareas.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); // Esto equivale a 'dotnet ef database update'
        Console.WriteLine(" Base de datos actualizada correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Error al crear la base de datos: {ex.Message}");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();