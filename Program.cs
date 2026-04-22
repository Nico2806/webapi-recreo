using Microsoft.EntityFrameworkCore;
using WebRecreo.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ CORS (UNA sola configuración)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend",
        policy =>
        {
            policy.WithOrigins("https://jolly-heliotrope-26141f.netlify.app")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ ORDEN CORRECTO
app.UseCors("PermitirFrontend");

app.UseSwagger();
app.UseSwaggerUI();

// ❌ opcional: podés comentar esto en Render
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Para Render
app.Urls.Add("http://0.0.0.0:8080");

app.Run();