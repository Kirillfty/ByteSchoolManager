using ByteSchoolManager;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using ClubsBack;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddTransient<ILessonsRepository, LessonRepository>();

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Описание схемы авторизации
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Введите только сам JWT-токен (префикс Bearer добавится автоматически).",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Глобальное требование для всех операций
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
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
//if (app.Environment.IsDevelopment())
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
//        await context.Database.MigrateAsync();
//    }
//}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
