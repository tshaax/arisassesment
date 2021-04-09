using System.Collections.Generic;

using Newtonsoft.Json;

namespace Aris.ServerTest.Models
{
    public class KoreUserLinks
    {
        public const string RegisterLink = "register";
        public const string ResetPasswordLink = "reset_password";
        public const string HelpChatLink = "help_chat";

        [JsonProperty("_actions")]
        public Dictionary<string, KoreLink> Actions { get; set; }

        [JsonProperty("_links")]
        public Dictionary<string, KoreLink> Links { get; set; }

    }
}
