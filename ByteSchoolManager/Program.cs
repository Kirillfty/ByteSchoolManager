using ByteSchoolManager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationContext>(u => u.UseNpgsql(conn));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        await context.Database.MigrateAsync();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
