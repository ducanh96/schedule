using EchoBot.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EchoBot.PromptUsers
{
    public class CustomPromptBot : IBot
    {
        private readonly CustomPromptBotAccessors _accessors;
        private readonly ILogger _logger;
        private Dictionary<StatusButton, string> ValuesDisplay { get; set; } = new Dictionary<StatusButton, string>();



        public CustomPromptBot(CustomPromptBotAccessors accessors, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new System.ArgumentNullException(nameof(loggerFactory));
            }

            _logger = loggerFactory.CreateLogger<CustomPromptBot>();
            _logger.LogTrace("EchoBot turn start.");
            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));

            ValuesDisplay.Add(StatusButton.BeginTransition, "Bắt đầu vận chuyển");


        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // Get the state properties from the turn context.
                ConversationFlow flow = await _accessors.ConversationFlowAccessor.GetAsync(turnContext, () => new ConversationFlow());
                UserProfile profile = await _accessors.UserProfileAccessor.GetAsync(turnContext, () => new UserProfile());

                await FillOutUserProfileAsync(flow, profile, turnContext, cancellationToken);
                // Update state and save changes.
                await _accessors.ConversationFlowAccessor.SetAsync(turnContext, flow);
                await _accessors.ConversationState.SaveChangesAsync(turnContext);

                await _accessors.UserProfileAccessor.SetAsync(turnContext, profile);
                await _accessors.UserState.SaveChangesAsync(turnContext);
            }
        }


        private async Task SendIntroCardAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var response = turnContext.Activity.CreateReply();

            var card = new HeroCard();
            card.Title = "Hệ thống hỗ trợ vận chuyển hàng lạnh";
            card.Text = @"Chào mừng bạn đến với dịch vụ trả lời tin nhắn tự động.\nHãy chọn 1 trong các chức năng sau:";
            card.Images = new List<CardImage>() { new CardImage(@"https://i.imgur.com/ygzfs2R.png") };
            card.Buttons = new List<CardAction>()
            {

                new CardAction
                {
                    Title = "Vận chuyển đơn hàng",
                    Type = ActionTypes.ImBack,
                    Value = ValuesDisplay.GetValueOrDefault(StatusButton.BeginTransition),

                },
                new CardAction(ActionTypes.OpenUrl,
                    "Đến trang đặt hàng", null, "Order", "Ask a question",
                    "https://stackoverflow.com/questions/tagged/botframework"),

            };

            response.Attachments = new List<Attachment>() { card.ToAttachment() };

            await turnContext.SendActivityAsync(response, cancellationToken);
        }

        private async Task FillOutUserProfileAsync(ConversationFlow flow, UserProfile profile, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            string message;
            string input = turnContext.Activity.Text?.Trim();
            if (!profile.IsStartConverstation)
            {
                profile.IsStartConverstation = true;
                await SendIntroCardAsync(turnContext, cancellationToken);

            }
            else
            {
                if (profile.IsStartTransition)
                {
                    switch (flow.LastDriverQuestion)
                    {

                        case DriverQuestions.USERNAME:
                            await turnContext.SendActivityAsync($"Dit me");
                            if (ValidateName(input, out string name, out message))
                            {

                            }
                            else
                            {
                                await turnContext.SendActivityAsync(message ?? "I'm sorry, I didn't understand that.");

                            }

                            break;
                        case DriverQuestions.PASSWORD:
                            break;
                        default:
                            break;
                    }
                }
                else
                {

                    // kiêm tra nguoi dung có bấm nút bd ko
                    if (input.ToLower().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.BeginTransition).ToLower()))
                    {
                        profile.IsStartTransition = true;
                        await turnContext.SendActivityAsync($"Chào bạn");
                        await turnContext.SendActivityAsync($"Bạn vui lòng nhập tên đăng nhập");
                        flow.LastDriverQuestion = DriverQuestions.USERNAME;
                    }

                }

            }

            //switch (flow.LastQuestionAsked)
            //{

            //    case ConversationFlow.Question.None:
            //        await SendIntroCardAsync(turnContext, cancellationToken);
            //        flow.LastQuestionAsked = ConversationFlow.Question.Name;
            //        break;
            //    case ConversationFlow.Question.Name:
            //        if (ValidateName(input, out string name, out message))
            //        {
            //            profile.Name = name;
            //            await turnContext.SendActivityAsync($"Hi {profile.Name}.");
            //            await turnContext.SendActivityAsync("How old are you?");
            //            flow.LastQuestionAsked = ConversationFlow.Question.Age;
            //            break;
            //        }
            //        else
            //        {
            //            await turnContext.SendActivityAsync(message ?? "I'm sorry, I didn't understand that.");
            //            break;
            //        }

            //    case ConversationFlow.Question.Age:
            //        if (ValidateAge(input, out int age, out message))
            //        {
            //            profile.Age = age;
            //            await turnContext.SendActivityAsync($"I have your age as {profile.Age}.");
            //            await turnContext.SendActivityAsync("When is your flight?");
            //            flow.LastQuestionAsked = ConversationFlow.Question.Date;
            //            break;
            //        }
            //        else
            //        {
            //            await turnContext.SendActivityAsync(message ?? "I'm sorry, I didn't understand that.");
            //            break;
            //        }

            //    case ConversationFlow.Question.Date:
            //        if (ValidateDate(input, out string date, out message))
            //        {
            //            profile.Date = date;
            //            await turnContext.SendActivityAsync($"Your cab ride to the airport is scheduled for {profile.Date}.");
            //            await turnContext.SendActivityAsync($"Thanks for completing the booking {profile.Name}.");
            //            await turnContext.SendActivityAsync($"Type anything to run the bot again.");
            //            flow.LastQuestionAsked = ConversationFlow.Question.None;
            //            profile = new UserProfile();
            //            break;
            //        }
            //        else
            //        {
            //            await turnContext.SendActivityAsync(message ?? "I'm sorry, I didn't understand that.");
            //            break;
            //        }
            //}
        }

        private static bool ValidateName(string input, out string name, out string message)
        {
            name = null;
            message = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                message = "Please enter a name that contains at least one character.";
            }
            else
            {
                name = input.Trim();
            }

            return message is null;
        }
        private static bool ValidateAge(string input, out int age, out string message)
        {
            age = 0;
            message = null;

            // Try to recognize the input as a number. This works for responses such as "twelve" as well as "12".
            try
            {
                // Attempt to convert the Recognizer result to an integer. This works for "a dozen", "twelve", "12", and so on.
                // The recognizer returns a list of potential recognition results, if any.
                List<ModelResult> results = NumberRecognizer.RecognizeNumber(input, Culture.English);
                foreach (var result in results)
                {
                    // result.Resolution is a dictionary, where the "value" entry contains the processed string.
                    if (result.Resolution.TryGetValue("value", out object value))
                    {
                        age = Convert.ToInt32(value);
                        if (age >= 18 && age <= 120)
                        {
                            return true;
                        }
                    }
                }

                message = "Please enter an age between 18 and 120.";
            }
            catch
            {
                message = "I'm sorry, I could not interpret that as an age. Please enter an age between 18 and 120.";
            }

            return message is null;
        }
        private static bool ValidateDate(string input, out string date, out string message)
        {
            date = null;
            message = null;

            // Try to recognize the input as a date-time. This works for responses such as "11/14/2018", "9pm", "tomorrow", "Sunday at 5pm", and so on.
            // The recognizer returns a list of potential recognition results, if any.
            try
            {
                List<ModelResult> results = DateTimeRecognizer.RecognizeDateTime(input, Culture.English);

                // Check whether any of the recognized date-times are appropriate,
                // and if so, return the first appropriate date-time. We're checking for a value at least an hour in the future.
                DateTime earliest = DateTime.Now.AddHours(1.0);
                foreach (ModelResult result in results)
                {
                    // result.Resolution is a dictionary, where the "values" entry contains the processed input.
                    var resolutions = result.Resolution["values"] as List<Dictionary<string, string>>;
                    foreach (var resolution in resolutions)
                    {
                        // The processed input contains a "value" entry if it is a date-time value, or "start" and
                        // "end" entries if it is a date-time range.
                        if (resolution.TryGetValue("value", out string dateString)
                            || resolution.TryGetValue("start", out dateString))
                        {
                            if (DateTime.TryParse(dateString, out var candidate)
                                && earliest < candidate)
                            {
                                date = candidate.ToShortDateString();
                                return true;
                            }
                        }
                    }
                }

                message = "I'm sorry, please enter a date at least an hour out.";
            }
            catch
            {
                message = "I'm sorry, I could not interpret that as an appropriate date. Please enter a date at least an hour out.";
            }

            return false;
        }
        public enum StatusButton
        {
            BeginTransition,

        }
    }

}
