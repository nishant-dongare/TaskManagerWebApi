using Microsoft.EntityFrameworkCore;
using Taskmanager_MVCAPI;
using Taskmanager_MVCAPI.Data;
using Taskmanager_MVCAPI.Repo;
using Taskmanager_MVCAPI.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));


builder.Services.AddScoped<taskInterface, TaskService>();
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<IBatchRepository, BatchService>();
builder.Services.AddScoped<IStudentRepository, StudentService>();
builder.Services.AddScoped<INewTaskRepository, NewTaskService>();




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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
