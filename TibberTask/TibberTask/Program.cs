using Microsoft.EntityFrameworkCore;
using TibberTask.PG;
using TibberTask.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("TibberTask");
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ExecutionCont>(
    options => 
        options.UseNpgsql(connectionString)
        .UseLowerCaseNamingConvention()
        );

builder.Services.AddSingleton<ExecutionRepo>(); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
