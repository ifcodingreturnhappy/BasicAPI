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
                output.DISCORD_WEBHOOK = configuration.GetSection(nameof(AuthSettings.DISCORD_WEBHOOK)).Value;
                output.DISCORD_USERNAME = configuration.GetSection(nameof(AuthSettings.DISCORD_USERNAME)).Value;
            }
            catch (System.Exception)
            {
                // Log
            }

            return output;
        }

        public string AUTH_SECRET { get; set; }
        public string DISCORD_WEBHOOK { get; set; }
        public string DISCORD_USERNAME { get; set; }
    }
}
