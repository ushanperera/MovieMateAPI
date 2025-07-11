using MovieMateAPI.Dependencies;
using MovieMateAPI.Dependencies.Configs;
using MovieMateAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

//--------------------------Add services (DI) to app---------------------
builder.AddDependencies();

builder.AddDummyDataServices(); // for the static enpoints available in this API (similer to API provide)
builder.AddWebjetApiServices(); // ths will fetch data from Webjet API for lower movie price


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

