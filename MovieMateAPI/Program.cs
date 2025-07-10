using MovieMateAPI.Dependencies;
using MovieMateAPI.Dependencies.Configs;
using MovieMateAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

//--------------------------Add services (DI) to app---------------------
builder.AddDependencies();

builder.AddDummyDataServices();
builder.AddWebjetApiServices();


//--------------------------Middleware-----------------------------------
var app = builder.Build();
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
app.UseAppOpenApi();

// add App Endpoints service Middleware
app.UseAppEndpoints();

// add CORES Middleware
app.UseAppCoresConfig();


app.Run();

