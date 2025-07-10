using MovieMateAPI;
using MovieMateAPI.Dependencies;
using MovieMateAPI.Dependencies.Configs;


var builder = WebApplication.CreateBuilder(args);

//--------------------------Add services (DI) to app---------------------
builder.AddAllDependencies();

//--------------------------Middleware--------------------------------
var app = builder.Build();
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
app.UseAppOpenApi();

// add App Endpoints service Middleware
app.UseAppEndpoints();

// add CORES Middleware
app.UseAppCoresConfig();


app.Run();

