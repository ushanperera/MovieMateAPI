namespace MovieMateAPI.Dependencies.Configs
{
    public static class CoresConfig
    {
        private const string CoresPolicy = "AllowAll";
        public static void AddCoresServises(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CoresPolicy,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
        }
        public static void UseAppCoresConfig(this IApplicationBuilder app)
        {
            app.UseCors(CoresPolicy);
        }
    }
}
