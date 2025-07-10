using MovieMateAPI.Dependencies.Configs;

namespace MovieMateAPI.Dependencies
{
    public static class DependenciesConfig
    {
        //All Dependencies will be listed here
        public static void AddAllDependencies(this WebApplicationBuilder builder)
        {   
            //Open API service added
            builder.Services.AddOpenApi();

            //Add the Cores services
            builder.Services.AddCoresServises(); 

        }
    }
}
