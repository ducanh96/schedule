using EchoBot.DialogPrompt.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.DialogPrompt
{
    public class DialogPromptBotAccessors
    {
        public ConversationState ConversationState { get; }
        public DialogPromptBotAccessors(ConversationState conversationState)
        {
            ConversationState = conversationState
                ?? throw new ArgumentNullException(nameof(conversationState));
        }
        public static string DialogStateAccessorKey { get; }
        = "DialogPromptBotAccessors.DialogState";
        public static string ReservationAccessorKey { get; }
            = "DialogPromptBotAccessors.Reservation";
        public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }
        public IStatePropertyAccessor<Reservation> ReservationAccessor { get; set; }
    }
}
