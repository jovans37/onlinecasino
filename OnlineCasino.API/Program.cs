using OnlineCasino.API.Extensions;
using OnlineCasino.Application.DependencyInjection;
using OnlineCasino.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerWithAuthorization();

//add the infrastructure services and dependencies
builder.Services.AddInfrastructure(builder.Configuration);

//add application services
builder.Services.AddApplication(); 

builder.Services.AddHttpContextAccessor();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

//for demo purposes, on run of the application the migrations will be applied
//on production migration should be runned in the release pipeline
app.ApplyMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
