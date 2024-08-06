using Cuidador.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var local = "cadenaSQLLocal";
var remota = "cadenaSQLRemota";

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString(remota);

// AGERGAMOS LA CONFIGURACION SQL
builder.Services.AddDbContext<SysCuidadorV2Context>(options => options.UseSqlServer(connectionString));

// DEFINIMOS LA NUEVA POLITICA DE LOS CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("nuevaPolitica", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar CORS
app.UseCors("nuevaPolitica");

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
