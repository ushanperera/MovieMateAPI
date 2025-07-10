namespace MovieMateAPI.Dependencies.Configs
{
    public static class OpenApiConfig
    {
        public static void AddOpenApiServices(this IServiceCollection services)
        {
            services.AddOpenApi();
        }

        public static void UseAppOpenApi(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            //else
            //{
            //    app.MapOpenApi();
            //    // rest of the logic here
            //}

        }
    }
}
