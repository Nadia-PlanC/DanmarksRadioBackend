using Danmarks_Radio.EFDB;
using Danmarks_Radio.Interfaces;
using Danmarks_Radio.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Add CORS service and configure policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()   // Allow any origin to access
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddAuthorization();

// Tilføj swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Indtast din JWT token"
    });
    opt.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", doc),
            new List<string>()
        }
    });
});

// Database (Entity Framework)
var connectionString = builder.Configuration["ConnectionString"]; // secret json
builder.Services.AddDbContext<EFDBContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<IMusicRepo, DbMusicRepo>(); // Transient, fordi data ligger i databasen
//builder.Services.AddSingleton<IMusicRepo>(new MusicListRepo(includeData: true)); // List version
builder.Services.AddTransient<IUserRepo, DbUserRepo>(); // Transient, fordi data ligger i databasen
//builder.Services.AddSingleton<IUserRepo>(new UserListRepo(includeData: true)); // List version

var app = builder.Build();

// Swagger Setup
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 2. Define that CORS should be used!
app.UseCors("AllowAll");

// JWT Setup
app.UseAuthentication(); // "Who are you?"
app.UseAuthorization();  // "Are you allowed to be here?"

app.MapControllers();

app.Run();
