namespace NZWalks.API.Tests.Configuration
{
    public static class TestConfiguration
    {
        public static string ApiBaseUrl
        {
            get
            {
                var baseUrl = Environment.GetEnvironmentVariable("API_BASE_URL");
                return string.IsNullOrEmpty(baseUrl) ? "http://localhost:5029" : baseUrl;
            }
        }

        public static int TimeoutSeconds
        {
            get
            {
                var timeout = Environment.GetEnvironmentVariable("API_TIMEOUT_SECONDS");
                return string.IsNullOrEmpty(timeout) ? 30 : int.Parse(timeout);
            }
        }
    }
}
