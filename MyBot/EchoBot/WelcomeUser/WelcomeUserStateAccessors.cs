using EchoBot.Model;
using EchoBot.WelcomeUser.Model;
using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.WelcomeUser
{
    public class WelcomeUserStateAccessors
    {
        public IStatePropertyAccessor<WelcomeUserState> WelcomeUserState { get; set; }
        public IStatePropertyAccessor<ConversationDriverFlow> ConversationDriverFlowAccessor { get; set; }
        public ConversationState ConversationState { get; }
        public static string ConversationDriverFlowName { get; } = "ConversationDriverFlow";
        public UserState UserState { get; }
        public static string WelcomeUserName { get; } = $"{nameof(WelcomeUserStateAccessors)}.WelcomeUserState";

        public WelcomeUserStateAccessors(UserState userState, ConversationState conversationState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));
        }
    }
}
