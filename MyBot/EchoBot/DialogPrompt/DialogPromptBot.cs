using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EchoBot.DialogPrompt
{
    public class DialogPromptBot
    {
        // Define identifiers for our dialogs and prompts.
        private const string ReservationDialog = "reservationDialog";
        private const string PartySizePrompt = "partySizePrompt";
        private const string SizeRangePrompt = "sizeRangePrompt";
        private const string LocationPrompt = "locationPrompt";
        private const string ReservationDatePrompt = "reservationDatePrompt";

        // Define keys for tracked values within the dialog.
        private const string LocationKey = "location";
        private const string PartySizeKey = "partySize";


        private readonly DialogSet _dialogSet;
        private readonly DialogPromptBotAccessors _accessors;

        // Initializes a new instance of the <see cref="DialogPromptBot"/> class.
        public DialogPromptBot(DialogPromptBotAccessors accessors, ILoggerFactory loggerFactory)
        {
            // ...

            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));

            // Create the dialog set and add the prompts, including custom validation.
            _dialogSet = new DialogSet(_accessors.DialogStateAccessor);

            //_dialogSet.Add(new NumberPrompt<int>(PartySizePrompt, PartySizeValidatorAsync));
            //_dialogSet.Add(new NumberPrompt<int>(SizeRangePrompt, RangeValidatorAsync));
            //_dialogSet.Add(new ChoicePrompt(LocationPrompt));
            //_dialogSet.Add(new DateTimePrompt(ReservationDatePrompt, DateValidatorAsync));

            // Define the steps of the waterfall dialog and add it to the set.
            WaterfallStep[] steps = new WaterfallStep[]
            {
                
            };

            _dialogSet.Add(new WaterfallDialog(ReservationDialog, steps));
        }


    }
}
