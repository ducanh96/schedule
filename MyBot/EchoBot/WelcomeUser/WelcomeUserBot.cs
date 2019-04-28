using EchoBot.Model;
using EchoBot.WelcomeUser.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using TransitionApp.Service.Interface;

namespace EchoBot.WelcomeUser
{
    public class WelcomeUserBot : IBot
    {
        private readonly IDriverService _driverService;
        // Messages sent to the user.
        private const string WelcomeMessage = @"This is a simple Welcome Bot sample.This bot will introduce you
                                        to welcoming and greeting users. You can say 'intro' to see the
                                        introduction card. If you are running this bot in the Bot Framework
                                        Emulator, press the 'Start Over' button to simulate user joining
                                        a bot or a channel";
        private const string InfoMessage = @"You are seeing this message because the bot received at least one
                                    'ConversationUpdate' event, indicating you (and possibly others)
                                    joined the conversation. If you are using the emulator, pressing
                                    the 'Start Over' button to trigger this event again. The specifics
                                    of the 'ConversationUpdate' event depends on the channel. You can
                                    read more information at:
                                        https://aka.ms/about-botframework-welcome-user";

        private const string PatternMessage = @"It is a good pattern to use this event to send general greeting
                                        to user, explaining what your bot can do. In this example, the bot
                                        handles 'hello', 'hi', 'help' and 'intro. Try it now, type 'hi'";
        // The bot state accessor object. Use this to access specific state properties.
        private readonly WelcomeUserStateAccessors _welcomeUserStateAccessors;

        // Initializes a new instance of the <see cref="WelcomeUserBot"/> class.
        public WelcomeUserBot(WelcomeUserStateAccessors statePropertyAccessor, IDriverService driverService)
        {
            _driverService = driverService;
            _welcomeUserStateAccessors = statePropertyAccessor ?? throw new System.ArgumentNullException("state accessor can't be null");
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // use state accessor to extract the didBotWelcomeUser flag
            var didBotWelcomeUser = await _welcomeUserStateAccessors.WelcomeUserState.GetAsync(turnContext, () => new WelcomeUserState());
            ConversationDriverFlow flow = await _welcomeUserStateAccessors.ConversationDriverFlowAccessor.GetAsync(turnContext, () => new ConversationDriverFlow());
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // Your bot should proactively send a welcome message to a personal chat the first time
                // (and only the first time) a user initiates a personal chat with your bot.
                if (didBotWelcomeUser.DidSignIn == false)
                {
                    didBotWelcomeUser.DidSignIn = true;
                    // Update user state flag to reflect bot handled first user interaction.
                    await _welcomeUserStateAccessors.WelcomeUserState.SetAsync(turnContext, didBotWelcomeUser);
                    await _welcomeUserStateAccessors.UserState.SaveChangesAsync(turnContext);

                    // the channel should sends the user name in the 'From' object
                    var userName = turnContext.Activity.From.Name;

                    await turnContext.SendActivityAsync(
                        $"You are seeing this message because this was your first message ever to this bot.",
                        cancellationToken: cancellationToken);
                    await turnContext.SendActivityAsync(
                        $"It is a good practice to welcome the user and provide personal greeting. For example, welcome {userName}.",
                        cancellationToken: cancellationToken);
                }
                else
                {
                    // This example hard-codes specific utterances. You should use LUIS or QnA for more advanced language understanding.
                    var text = turnContext.Activity.Text.ToLowerInvariant();
                    switch (text)
                    {
                        case "hello":
                            //GetDriverResponse driverResponse = null;
                            //await RunAsync();
                            //HttpResponseMessage response = await client.GetAsync($"transition/Driver/{id}");
                            //if (response.IsSuccessStatusCode)
                            //{
                            //    driverResponse = await response.Content.ReadAsAsync<GetDriverResponse>();
                            //    Console.WriteLine(driverResponse.Data);

                            //    await turnContext.SendActivityAsync($"Name: {driverResponse.Message}, Code: {driverResponse.Success}", cancellationToken: cancellationToken);

                            //}
                            await SendIntroCardAsync(turnContext, cancellationToken);
                            //var driver = _driverService.Get(1);
                            //await turnContext.SendActivityAsync($"Data Driver: Name: {driver.Name}, Code: {driver.Code}", cancellationToken: cancellationToken);
                            break;
                        case "hi":
                            await turnContext.SendActivityAsync($"You said {text}.", cancellationToken: cancellationToken);
                            break;
                        case "intro":
                        case "help":
                            await SendIntroCardAsync(turnContext, cancellationToken);
                            break;
                        default:
                            await turnContext.SendActivityAsync(WelcomeMessage, cancellationToken: cancellationToken);
                            break;
                    }
                }
            }
            // Greet when users are added to the conversation.
            else if (turnContext.Activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (turnContext.Activity.MembersAdded != null)
                {
                    // Iterate over all new members added to the conversation
                    foreach (var member in turnContext.Activity.MembersAdded)
                    {
                        // Greet anyone that was not the target (recipient) of this message
                        // the 'bot' is the recipient for events from the channel,
                        // turnContext.Activity.MembersAdded == turnContext.Activity.Recipient.Id indicates the
                        // bot was added to the conversation.
                        if (member.Id != turnContext.Activity.Recipient.Id)
                        {
                            await turnContext.SendActivityAsync($"Hi there - {member.Name}. {WelcomeMessage}", cancellationToken: cancellationToken);
                            await turnContext.SendActivityAsync(InfoMessage, cancellationToken: cancellationToken);
                            await turnContext.SendActivityAsync(PatternMessage, cancellationToken: cancellationToken);
                        }
                    }
                }
            }
            else
            {
                // Default behavior for all other type of activities.
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} activity detected");
            }
            // save any state changes made to your state objects.
            await _welcomeUserStateAccessors.UserState.SaveChangesAsync(turnContext);
        }
        // Sends an adaptive card greeting.
        private static async Task SendIntroCardAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var response = turnContext.Activity.CreateReply();

            var card = new HeroCard();
            card.Title = "Welcome to Bot Framework!";
            card.Text = @"Welcome to Welcome Users bot sample! This Introduction card
                    is a great way to introduce your Bot to the user and suggest
                    some things to get them started. We use this opportunity to
                    recommend a few next steps for learning more creating and deploying bots.";
            card.Images = new List<CardImage>() { new CardImage("https://aka.ms/bf-welcome-card-image") };
            card.Buttons = new List<CardAction>()
            {
                new CardAction(ActionTypes.OpenUrl,
                    "Get an overview", null, "Get an overview", "Get an overview",
                    "https://docs.microsoft.com/en-us/azure/bot-service/?view=azure-bot-service-4.0"),
                new CardAction(ActionTypes.OpenUrl,
                    "Ask a question", null, "Ask a question", "Ask a question",
                    "https://stackoverflow.com/questions/tagged/botframework"),
                new CardAction(ActionTypes.OpenUrl,
                    "Learn how to deploy", null, "Learn how to deploy", "Learn how to deploy",
                    "https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-howto-deploy-azure?view=azure-bot-service-4.0"),
            };

            response.Attachments = new List<Attachment>() { card.ToAttachment() };
            await turnContext.SendActivityAsync(response, cancellationToken);
        }

    }

}