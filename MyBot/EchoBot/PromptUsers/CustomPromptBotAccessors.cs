using EchoBot.Model;
using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.PromptUsers
{
    public class CustomPromptBotAccessors
    {
        public static string ConversationFlowName { get; } = "ConversationFlow";
        public static string UserProfileName { get; } = "UserProfile";
        public IStatePropertyAccessor<ConversationFlow> ConversationFlowAccessor { get; set; }

        public IStatePropertyAccessor<UserProfile> UserProfileAccessor { get; set; }
        public ConversationState ConversationState { get; }
        public UserState UserState { get; }
        public CustomPromptBotAccessors(ConversationState conversationState, UserState userState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));
        }
    }
}
