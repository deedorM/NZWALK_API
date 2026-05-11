using RestSharp;

namespace NZWalks.API.Tests.Helpers
{
    /// <summary>
    /// Helper class for common test operations and utilities
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Create a sample region for testing
        /// </summary>
        public static CreateRegionRequest CreateSampleRegion(
            string code = "TEST",
            string name = "Test Region",
            string imageUrl = "https://example.com/image.jpg")
        {
            return new CreateRegionRequest
            {
                Code = code,
                Name = name,
                RegionImageUrl = imageUrl
            };
        }

        /// <summary>
        /// Verify if API response is successful
        /// </summary>
        public static bool IsResponseSuccessful(RestResponse response)
        {
            return response.StatusCode >= System.Net.HttpStatusCode.OK &&
                   response.StatusCode < System.Net.HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// Get error message from failed response
        /// </summary>
        public static string GetErrorMessage(RestResponse response)
        {
            if (response.IsSuccessful)
                return "Response was successful";

            return !string.IsNullOrEmpty(response.Content)
                ? response.Content
                : $"Status Code: {response.StatusCode}";
        }

        /// <summary>
        /// Wait for condition with timeout
        /// </summary>
        public static async Task<bool> WaitForCondition(
            Func<Task<bool>> condition,
            TimeSpan timeout,
            TimeSpan? pollingInterval = null)
        {
            pollingInterval ??= TimeSpan.FromMilliseconds(100);
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                if (await condition())
                    return true;

                await Task.Delay(pollingInterval.Value);
            }

            return false;
        }

        /// <summary>
        /// Retry API call with exponential backoff
        /// </summary>
        public static async Task<T> RetryAsync<T>(
            Func<Task<T>> operation,
            int maxRetries = 3,
            TimeSpan? initialDelay = null)
        {
            initialDelay ??= TimeSpan.FromMilliseconds(100);
            var delay = initialDelay.Value;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when (i < maxRetries - 1)
                {
                    await Task.Delay(delay);
                    delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 2);
                }
            }

            throw new InvalidOperationException($"Operation failed after {maxRetries} retries");
        }

        /// <summary>
        /// Generate unique test data
        /// </summary>
        public static string GenerateUniqueTestData(string prefix = "TEST")
        {
            return $"{prefix}_{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        /// <summary>
        /// Validate email format (if needed for future tests)
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Compare two objects field by field
        /// </summary>
        public static List<string> CompareObjects<T>(T obj1, T obj2) where T : class
        {
            var differences = new List<string>();
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                if (!prop.CanRead)
                    continue;

                var value1 = prop.GetValue(obj1);
                var value2 = prop.GetValue(obj2);

                if (!Equals(value1, value2))
                {
                    differences.Add(
                        $"Property '{prop.Name}': Expected '{value1}', Got '{value2}'");
                }
            }

            return differences;
        }
    }
}
