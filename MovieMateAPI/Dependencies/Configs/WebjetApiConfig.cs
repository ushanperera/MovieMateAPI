using MovieMateAPI.Services;
using Polly;
using Polly.Extensions.Http;

namespace MovieMateAPI.Dependencies.Configs
{
    public static class WebjetApiConfig
    {
        public static void AddWebjetApiServices(this WebApplicationBuilder builder)
        {
            // Service Registration-----------------------------------------------------------
            builder.Services.AddScoped<IMovieService, MovieService>();

            //HTTP Client Configuration(This one only needed when connecting to the API) -----
            var baseUrl = builder.Configuration["WebjetApi:BaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new InvalidOperationException("WebjetApi:BaseUrl is not configured.");
            }

            builder.Services.AddHttpClient("WebjetClient", client =>
            {
                client.BaseAddress = new Uri(baseUrl!);
                client.Timeout = TimeSpan.FromSeconds(2); // Timeout
            }).AddPolicyHandler(GetRetryPolicy()); // Retry policy method below
        }

        /// <summary>
        /// This is to configure the retry policy specifically forthe WebJect URL
        /// </summary>
        /// <returns></returns>
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds}s due to: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
                    });
        }
    }
}
