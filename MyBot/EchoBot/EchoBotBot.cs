// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using EchoBot.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot
{
    /// <summary>
    /// Represents a bot that processes incoming activities.
    /// For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    /// This is a Transient lifetime service.  Transient lifetime services are created
    /// each time they're requested. For each Activity received, a new instance of this
    /// class is created. Objects that are expensive to construct, or have a lifetime
    /// beyond the single turn, should be carefully managed.
    /// For example, the <see cref="MemoryStorage"/> object and associated
    /// <see cref="IStatePropertyAccessor{T}"/> object are created with a singleton lifetime.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    public class EchoBotBot : IBot
    {
        private readonly EchoBotAccessors _accessors;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="conversationState">The managed conversation state.</param>
        /// <param name="loggerFactory">A <see cref="ILoggerFactory"/> that is hooked to the Azure App Service provider.</param>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider"/>
        public EchoBotBot(ConversationState conversationState, ILoggerFactory loggerFactory, EchoBotAccessors echoBotAccessors)
        {
            if (conversationState == null)
            {
                throw new System.ArgumentNullException(nameof(conversationState));
            }

            if (loggerFactory == null)
            {
                throw new System.ArgumentNullException(nameof(loggerFactory));
            }

            //_accessors = new EchoBotAccessors(conversationState)
            //{
            //    CounterState = conversationState.CreateProperty<CounterState>(EchoBotAccessors.CounterStateName),
            //};

            #region Customize
            _accessors = echoBotAccessors ?? throw new System.ArgumentNullException(nameof(echoBotAccessors));
            _accessors.CounterState = conversationState.CreateProperty<CounterState>(EchoBotAccessors.CounterStateName);
            #endregion


            _logger = loggerFactory.CreateLogger<EchoBotBot>();
            _logger.LogTrace("Turn start.");
        }

        /// <summary>
        /// Every conversation turn for our Echo Bot will call this method.
        /// There are no dialogs used, since it's "single turn" processing, meaning a single
        /// request and response.
        /// </summary>
        /// <param name="turnContext">A <see cref="ITurnContext"/> containing all the data needed
        /// for processing this conversation turn. </param>
        /// <param name="cancellationToken">(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        /// <seealso cref="BotStateSet"/>
        /// <seealso cref="ConversationState"/>
        /// <seealso cref="IMiddleware"/>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Handle Message activity type, which is the main activity type for shown within a conversational interface
            // Message activities may contain text, speech, interactive cards, and binary or unknown attachments.
            // see https://aka.ms/about-bot-activity-message to learn more about the message and other activity types
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {

                #region Prompt users for input

                 





                #endregion


                // Get the conversation state from the turn context.
                var state = await _accessors.CounterState.GetAsync(turnContext, () => new CounterState());

                // Bump the turn count for this conversation.
                state.TurnCount++;
                //if (!state.SayHello)
                //{
                //    string strMessage = $"Hello World! {System.Environment.NewLine}";
                //    strMessage += "Take to me and I will repeat it back!";
                //    await turnContext.SendActivityAsync(strMessage);
                //    // set SaidHello
                //    state.SayHello = true;
                //}

                #region Customize

                #region Save users for input

                // Get the state properties from the turn context.
                UserProfile userProfile = await _accessors.UserProfileAccessor.GetAsync(turnContext, () => new UserProfile());
                ConversationData conversationData = await _accessors.ConversationDataAccessor.GetAsync(turnContext, () => new ConversationData());

                if (string.IsNullOrEmpty(userProfile.Name))
                {
                    // First time around this is set to false, so we will prompt user for name.
                    if (conversationData.PromptedUserForName)
                    {
                        // set the name to what the user provided
                        userProfile.Name = turnContext.Activity.Text?.Trim();

                        // Acknowledge that we got their name.
                        await turnContext.SendActivityAsync($"Thanks {userProfile.Name}.");

                        // Reset the flag to allow the bot to go though the cycle again.
                        conversationData.PromptedUserForName = false;
                    }

                    else
                    {
                        // Prompt the user for their name.
                        await turnContext.SendActivityAsync($"What is your name?");

                        // Set the flag to true, so we don't prompt in the next turn.
                        conversationData.PromptedUserForName = true;
                    }

                    // Save user state and save changes.
                    await _accessors.UserProfileAccessor.SetAsync(turnContext, userProfile);
                    await _accessors.UserState.SaveChangesAsync(turnContext);
                }
                else
                {
                    // Add message details to the conversation data.
                    conversationData.Timestamp = turnContext.Activity.Timestamp.ToString();
                    conversationData.ChannelId = turnContext.Activity.ChannelId.ToString();

                    // Display state data
                    await turnContext.SendActivityAsync($"{userProfile.Name} sent: {turnContext.Activity.Text}");
                    await turnContext.SendActivityAsync($"Message received at: {conversationData.Timestamp}");
                    await turnContext.SendActivityAsync($"Message received from: {conversationData.ChannelId}");
                }

                // Update conversation state and save changes.
                await _accessors.ConversationDataAccessor.SetAsync(turnContext, conversationData);
                await _accessors.ConversationState.SaveChangesAsync(turnContext);

                #endregion

                #region dua ra nut goi y cho nguoi dung
                //var reply = turnContext.Activity.CreateReply("What is your favorite color?");
                //reply.SuggestedActions = new SuggestedActions()
                //{
                //    Actions = new List<CardAction>()
                //    {
                //        new CardAction()
                //        {
                //            Title = "Red",
                //            Type = ActionTypes.ImBack,
                //            Value = "Red"
                //        },
                //        new CardAction()
                //        {
                //            Title = "Yellow",
                //            Type = ActionTypes.ImBack,
                //            Value = "Yellow"
                //        },
                //        new CardAction()
                //        {
                //            Title = "Blue",
                //            Type = ActionTypes.ImBack,
                //            Value = "Blue"
                //        }

                //    }
                //};
                //await turnContext.SendActivityAsync(reply, cancellationToken: cancellationToken);
                #endregion



                #endregion

                // Set the property using the accessor.
                await _accessors.CounterState.SetAsync(turnContext, state);

                // Save the new turn count into the conversation state.
                await _accessors.ConversationState.SaveChangesAsync(turnContext);

                // Echo back to the user whatever they typed.
                //var responseMessage = $"Turn {state.TurnCount}: You sent '{turnContext.Activity.Text}'\n";
                //await turnContext.SendActivityAsync(responseMessage);
            }
            else
            {

                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected");
            }
        }
    }
}
