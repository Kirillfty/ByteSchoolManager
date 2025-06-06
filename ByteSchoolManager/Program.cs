using ByteSchoolManager;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using ClubsBack;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
string conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationContext>(u => u.UseNpgsql(conn));

IConfigurationSection authConfiguration = builder.Configuration.GetSection("AuthOptions");
AuthOptions authOptions = new AuthOptions(
             authConfiguration["ISSUER"] ?? throw new Exception("ISSUER is null!"),
             authConfiguration["AUDIENCE"] ?? throw new Exception("AUDIENCE is null!"),
             authConfiguration["KEY"]) ?? throw new Exception("KEY is null!");
builder.Services.AddSingleton<AuthOptions>(authOptions);
builder.Services.AddTransient<JwtCreator>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICoachRepository, CoachRepository>();
builder.Services.AddTransient<ICourseRepository, CourseRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = authOptions.Issuer, //???????? ????????
        ValidateAudience = true,
        ValidAudience = authOptions.Audience,//???????? ?????????
        ValidateLifetime = true,
        IssuerSigningKey = authOptions.GetSymmetricKey,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));



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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
