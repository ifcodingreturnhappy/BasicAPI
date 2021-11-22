using Microsoft.Extensions.Configuration;

namespace API
{
    public class AuthSettings
    {
        public static AuthSettings GetSettings(IConfiguration configuration)
        {
            var output = new AuthSettings();

            try
            {
                output.AUTH_SECRET = configuration.GetSection(nameof(AuthSettings.AUTH_SECRET)).Value;
            }
            catch (System.Exception)
            {
                // Log
            }

            return output;
        }

        public string AUTH_SECRET { get; set; }
    }
}
