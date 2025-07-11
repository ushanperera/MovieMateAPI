using MovieMateAPI.Dependencies.Configs;
using MovieMateAPI.Repository;
using Serilog;

namespace MovieMateAPI.Dependencies
{
    public static class DependenciesConfig
    {
        //All Dependencies will be listed here
        public static void AddDependencies(this WebApplicationBuilder builder)
        {   
            //Open API service added
            builder.Services.AddOpenApiServices();

            //Add the Cores services
            builder.Services.AddCoresServises();

        }
    }
}
