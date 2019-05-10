using AdaptiveCards;
using EchoBot.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Number;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TransitionApp.Domain.ReadModel.Account;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Service.Interface;

namespace EchoBot.PromptUsers
{
    public class CustomPromptBot : IBot
    {
        private readonly IAccountService _accountService;
        private readonly IDriverService _driverService;
        private readonly IScheduleService _scheduleService;
        private readonly CustomPromptBotAccessors _accessors;
        private readonly ILogger _logger;
        private Dictionary<StatusButton, string> ValuesDisplay { get; set; } = new Dictionary<StatusButton, string>();

        // This array contains the file location of our adaptive cards
        private readonly string[] _cards =
        {
            Path.Combine(".", "Card", "Demo.json"),
            Path.Combine(".", "Card", "Channel.json"),
            Path.Combine(".", "Card", "MenuLoginSuccess.json"),
            Path.Combine(".", "Card", "ButtonTemplate.json"),
            Path.Combine(".", "Card", "TemplateListCus.json")

        };


        public CustomPromptBot(CustomPromptBotAccessors accessors, ILoggerFactory loggerFactory,
            IAccountService accountService, IDriverService driverService,
            IScheduleService scheduleService)
        {
            if (loggerFactory == null)
            {
                throw new System.ArgumentNullException(nameof(loggerFactory));
            }
            _accountService = accountService;
            _scheduleService = scheduleService;
            _driverService = driverService;
            _logger = loggerFactory.CreateLogger<CustomPromptBot>();
            _logger.LogTrace("EchoBot turn start.");
            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));

            ValuesDisplay.Add(StatusButton.BeginTransition, "Bắt đầu vận chuyển");
            ValuesDisplay.Add(StatusButton.INVOICE_NOW, "INVOICE_NOW");
            ValuesDisplay.Add(StatusButton.INVOICE_PAST, "INVOICE_PAST");
            ValuesDisplay.Add(StatusButton.BACK, "BACK");

        }

        /// <summary>
        /// Creates an <see cref="Attachment"/> that contains an <see cref="AdaptiveCard"/>.
        /// </summary>
        /// <param name="filePath">The path to the <see cref="AdaptiveCard"/> json file.</param>
        /// <returns>An <see cref="Attachment"/> that contains an adaptive card.</returns>
        private static Attachment CreateAdaptiveCardAttachment(string filePath)
        {
            var adaptiveCardJson = File.ReadAllText(filePath);
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }



        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {

            //if (turnContext.Activity.Type == ActivityTypes.Message)
            //{
            //    var reply = turnContext.Activity.CreateReply();



            //    var adaptiveCardJson = File.ReadAllText(this._cards[1]);


            //    await turnContext.SendActivityAsync(reply, cancellationToken);



            //}

            if (turnContext.Activity.Type == ActivityTypes.Message)
            {



                // adaptive
                //var cardAttachment = CreateAdaptiveCardAttachment(this._cards[0]);

                //var reply = turnContext.Activity.CreateReply();
                //reply.Text = $"submit data: {turnContext.Activity.Value}";
                //reply.Attachments = new List<Attachment>() { cardAttachment };


                //List<CardImage> cardImages1 = new List<CardImage>();
                //cardImages1.Add(new CardImage(url: "https://upload.wikimedia.org/wikipedia/en/a/a6/Bender_Rodriguez.png"));

                //List<CardAction> cardButtons1 = new List<CardAction>();
                //CardAction plButton1 = new CardAction()
                //{
                //    Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                //    Type = "openUrl",
                //    Title = "WikiPedia Page"
                //};

                //cardButtons1.Add(plButton1);
                //HeroCard plCard1 = new HeroCard()
                //{
                //    Title = "I'm a hero card",
                //    Subtitle = "Pig Latin Wikipedia Page",
                //    Images = cardImages1,
                //    Buttons = cardButtons1
                //};
                //Attachment plAttachment1 = plCard1.ToAttachment();
                //reply.Attachments.Add(plAttachment1);

                //// Card #Two
                //List<CardImage> cardImages2 = new List<CardImage>();
                //cardImages2.Add(new CardImage(url: "https://upload.wikimedia.org/wikipedia/en/archive/a/a9/20151112035044!Banyan_Tree_(_Shiv_Bajrang_Dham_Kishunpur).jpeg"));

                //List<CardAction> cardButtons2 = new List<CardAction>();
                //CardAction plButton2 = new CardAction()
                //{
                //    Value = "https://en.wikipedia.org/wiki/Pig_Latin",
                //    Type = "openUrl",
                //    Title = "WikiPedia Page"
                //};
                //cardButtons2.Add(plButton2);
                //HeroCard plCard2 = new HeroCard()
                //{
                //    Title = "I'm a hero card",
                //    Subtitle = "Pig Latin Wikipedia Page",
                //    Images = cardImages2,
                //    Buttons = cardButtons2
                //};

                //Attachment plAttachment2 = plCard2.ToAttachment();
                //reply.Attachments.Add(plAttachment2);
                //reply.AttachmentLayout = "carousel";

                ////////////////////////////////////////////////////////


                //reply.AttachmentLayout = "carousel";
                //await turnContext.SendActivityAsync(reply, cancellationToken);



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
            card.Text = $"Chào mừng bạn đến với dịch vụ trả lời tin nhắn tự động.{Environment.NewLine}";
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
            Console.WriteLine(turnContext.Activity.Text);
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
                            if (ValidateUserName(input, out string username, out message))
                            {
                                profile.Username = username;
                                await turnContext.SendActivityAsync($"Bạn vui lòng nhập mật khẩu");
                                flow.LastDriverQuestion = DriverQuestions.PASSWORD;

                            }
                            else
                            {
                                await turnContext.SendActivityAsync(message ?? "Xin lỗi, tôi không hiểu điều bạn đang nói!!");
                            }
                            break;
                        case DriverQuestions.PASSWORD:
                            if (ValidatePassword(input, profile.Username, out string password, out message, out AccountReadModel account))
                            {
                                profile.Password = password;
                                profile.Account = account;
                                profile.Driver = _driverService.GetByAccount(account.Id);
                                var replyPassword = turnContext.Activity.CreateReply();
                                // tao typing on 
                                replyPassword.ChannelData = new
                                {
                                    sender_action = "typing_on"
                                };

                                replyPassword.Text = $"Đăng nhập thành công";

                                await turnContext.SendActivityAsync(replyPassword);
                                string dataMenu = File.ReadAllText(this._cards[3]);

                                Console.WriteLine(dataMenu);

                                replyPassword.ChannelData = JsonConvert.DeserializeObject(dataMenu);
                                replyPassword.Text = String.Empty;
                                await turnContext.SendActivityAsync(replyPassword, cancellationToken);
                                Console.WriteLine(turnContext.Activity);
                                flow.LastDriverQuestion = DriverQuestions.INVOICE_CHOOSE;

                            }
                            else
                            {
                                await turnContext.SendActivityAsync(message ?? "Xin lỗi, tôi không hiểu điều bạn đang nói!!");
                            }
                            break;
                        case DriverQuestions.INVOICE_CHOOSE:
                            if (input.ToLower().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.INVOICE_NOW).ToLower()))
                            {
                                flow.LastDriverQuestion = DriverQuestions.INVOICE_NOW;
                             
                                var customerReadModel = _driverService.GetCustomers(new DateTime(2019, 4, 24), profile.Driver.ID);

                                Console.WriteLine(JsonConvert.SerializeObject(customerReadModel));
                                Activity activity = turnContext.Activity.CreateReply();
                                var data = JsonConvert.SerializeObject(WriteReadCustomerFb(customerReadModel));
                                activity.ChannelData = JsonConvert.DeserializeObject(data);
                                Console.WriteLine(activity.ChannelData);

                                await turnContext.SendActivityAsync(activity, cancellationToken);
                            }
                            break;
                        case DriverQuestions.INVOICE_NOW:
                            string payLoad = SplitCustomerPayLoad(input.Trim(), Key.CUSTOMER);
                            Console.WriteLine(payLoad);
                            if (payLoad.Length > 0)
                            {
                                int customerId = int.Parse(payLoad);
                                var customerDetail = _scheduleService.GetInforCustomerOfRoute(customerId);
                                profile.Data.CustomerDetail = customerDetail;
                                var rep = turnContext.Activity.CreateReply();
                                rep.ChannelData = DetailCustomer(customerDetail);
                                // chuyen sang lua chon chuc nang
                                flow.LastDriverQuestion = DriverQuestions.CHOOSE_FUNCTIONAL_CUSTOMER;

                                await turnContext.SendActivityAsync(rep);

                            }
                            else
                            {

                            }
                            break;
                        case DriverQuestions.CHOOSE_FUNCTIONAL_CUSTOMER:
                            if (input.ToLower().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.MAPS).ToLower()))
                            {
                                
                            }
                            else if (input.Trim().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.ITEMS)))
                            {

                            }
                            else if (input.Trim().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.CALL)))
                            {

                            }
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
        #region View Facebook

        // lay danh sach khach hang
        private object WriteReadCustomerFb(IEnumerable<CustomerReadModel> customerReadModels)
        {
            List<object> list = new List<object>();
            foreach (var item in customerReadModels)
            {
                list.Add(new
                {
                    title = item.Name,
                    subtitle = item.PhoneNumber,
                    buttons = new[]
                        {
                            new
                            {
                                title = "Xem chi tiết",
                                type = "postback",
                                payload = Key.CUSTOMER.ToString() +"-"+item.Id
                            }
                        }
                });
            }
            return new
            {
                notification_type = "REGULAR",
                attachment = new
                {
                    type = "template",
                    payload = new
                    {
                        template_type = "list",
                        top_element_style = "compact",
                        elements = list,
                        buttons = new[]
                        {
                            new
                            {
                                title =  "Quay lại",
                                type = "postback",
                                payload = "BACK"
                            }
                        }
                    }
                }
            };
        }

        private object DetailCustomer(CustomerDetailReadModel customerDetail)
        {
            Console.WriteLine(customerDetail.Customer.PhoneNumber.ToString());
            return new
            {
                notification_type = "REGULAR",
                attachment = new
                {
                    type = "template",
                    payload = new
                    {
                        template_type = "button",
                        text = customerDetail.Customer.Name + Environment.NewLine +
                        string.Format("{0}-{1}-{2}-{3}",customerDetail.Address.StreetNumber,
                            customerDetail.Address.Street,customerDetail.Address.District,customerDetail.Address.City),
                        buttons = new[]
                        {
                            new
                            {
                                title = "Đơn hàng cần chuyển",
                                type = "postback",
                                payload = StatusButton.ITEMS.ToString()
                            },
                            new
                            {
                                title = "Xem chỉ đường",
                                type = "postback",
                                payload = StatusButton.MAPS.ToString()
                            },
                              new
                            {
                                title = "Gọi điện",
                                type = "phone_number",
                                payload = string.Format("+84{0}",customerDetail.Customer.PhoneNumber.Substring(1))
                            }


                        }
                    }
                }
            };
        }

        #endregion

        /// <summary>
        /// lay ra 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string SplitCustomerPayLoad(string payload, Key key)
        {
            var isCorrect = payload.Contains(key.ToString());
            return isCorrect ? payload.Split('-').GetValue(1).ToString() : string.Empty;
        }
        private bool ValidateUserName(string input, out string username, out string message)
        {
            username = null;
            message = null;

            if (string.IsNullOrWhiteSpace(input))
            {
                message = "Hãy nhập ít nhất 1 ký tự.";
            }
            else
            {
                username = input.Trim();
                var isExisit = _accountService.IsExistAccount(username);
                message = isExisit ? null : "Tài khoản không tồn tại.Vui lòng nhập lại!";
            }

            return message is null;
        }

        private bool ValidatePassword(string input, string usename, out string password, out string message, out AccountReadModel account)
        {
            password = null;
            message = null;
            account = null;
            if (string.IsNullOrWhiteSpace(input))
            {
                message = "Hãy nhập ít nhất 1 ký tự.";
            }
            else
            {
                password = input.Trim();
                account = _accountService.Get(usename, password);
                message = account == null ? "Sai mật khẩu, vui lòng nhập lại!" : null;

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
            INVOICE_NOW,
            INVOICE_PAST,
            BACK,
            ADDRESS,
            //--------------------
            MAPS,
            CALL,
            ITEMS
        }
        public enum Key
        {
            CUSTOMER,
            
        }
    }

}
