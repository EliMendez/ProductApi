using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Mapper;
using ProductApi.Repository.Interface;
using ProductApi.Repository.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CORS support
//You can enable one, multiple or any domain
//You use (*) to identify that any domain will be allowed
builder.Services.AddCors(d => d.AddPolicy("CORSApi", build =>
{
    build.WithOrigins("http://localhost:8000").AllowAnyMethod().AllowAnyHeader();
}));

//Add Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Add AutoMapper
builder.Services.AddAutoMapper(typeof(ProductMapper));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//CORS support
app.UseCors("CORSApi");

app.UseAuthorization();

app.MapControllers();

app.Run();
