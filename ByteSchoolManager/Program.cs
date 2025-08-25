using System.Diagnostics;
using ByteSchoolManager;
using ByteSchoolManager.Entities;
using ByteSchoolManager.Repository;
using ClubsBack;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
string conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationDbContext>(u => u.UseNpgsql(conn));

IConfigurationSection authConfiguration = builder.Configuration.GetSection("AuthOptions");
AuthOptions authOptions = new AuthOptions(
    authConfiguration["ISSUER"] ?? throw new Exception("ISSUER is null!"),
    authConfiguration["AUDIENCE"] ?? throw new Exception("AUDIENCE is null!"),
    authConfiguration["KEY"]) ?? throw new Exception("KEY is null!");
builder.Services.AddSingleton<AuthOptions>(authOptions);
builder.Services.AddTransient<JwtCreator>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICoachRepository, CoachRepository>();
builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddTransient<ILessonsRepository, LessonRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<RepositoryBase<Course>, CourseBaseRepository>();
builder.Services.AddScoped<RepositoryBase<Lesson>, LessonBaseRepository>();
builder.Services.AddScoped<RepositoryBase<StudentLesson>, StudentLessonBaseRepository>();
builder.Services.AddScoped<RepositoryBase<StudentCourse>, StudentCourseBaseRepository>();
builder.Services.AddScoped<RepositoryBase<Student>, StudentBaseRepository>();
builder.Services.AddScoped<RepositoryBase<Coach>, CoachBaseRepository>();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = authOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = authOptions.Audience,
        ValidateLifetime = true,
        IssuerSigningKey = authOptions.GetSymmetricKey,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    
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

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance =
            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});
builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }
}

app.UseExceptionHandler();

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