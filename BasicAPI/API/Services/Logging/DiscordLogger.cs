using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Services.Logging
{
    public class DiscordLogger // : ILogger => to implement
    {
        private readonly string _webhookUrl;
        private readonly string _username;
        public DiscordLogger(AuthSettings authSettings)
        {
            _webhookUrl = authSettings.DISCORD_WEBHOOK;
            _username = authSettings.DISCORD_USERNAME;
        }

        public void Log(string message)
        {
            Post(_webhookUrl, new NameValueCollection
            {
                {
                    "username",
                    _username
                },
                {
                    "content",
                    message
                }
            });
        }

        // Add async
        private byte[] Post(string uri, NameValueCollection data)
        {
            using(var webclient = new WebClient())
            {
                return webclient.UploadValues(uri, data);
            }
        }
    }
}
