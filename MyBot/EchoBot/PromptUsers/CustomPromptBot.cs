﻿using AdaptiveCards;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Commands.Schedule;
using TransitionApp.Domain.ReadModel.Account;
using TransitionApp.Domain.ReadModel.Customer;
using TransitionApp.Domain.ReadModel.Invoice;
using TransitionApp.Service.Interface;

namespace EchoBot.PromptUsers
{
    public class CustomPromptBot : IBot
    {
        private readonly IAccountService _accountService;
        private readonly IDriverService _driverService;
        private readonly IScheduleService _scheduleService;
        private readonly IInvoiceService _invoiceService;
        private readonly IMediatorHandler _bus;
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
            IScheduleService scheduleService, IInvoiceService invoiceService)
        {
            if (loggerFactory == null)
            {
                throw new System.ArgumentNullException(nameof(loggerFactory));
            }
            _accountService = accountService;
            _scheduleService = scheduleService;
            _driverService = driverService;
            _invoiceService = invoiceService;
            _logger = loggerFactory.CreateLogger<CustomPromptBot>();
            _logger.LogTrace("EchoBot turn start.");
            _accessors = accessors ?? throw new System.ArgumentNullException(nameof(accessors));

            ValuesDisplay.Add(StatusButton.BeginTransition, "Bắt đầu vận chuyển");
            ValuesDisplay.Add(StatusButton.STATUS_INVOICE, "STATUS_INVOICE");

            ValuesDisplay.Add(StatusButton.INVOICE_NOW, "INVOICE_NOW");
            ValuesDisplay.Add(StatusButton.INVOICE_PAST, "INVOICE_PAST");
            ValuesDisplay.Add(StatusButton.CHANGE_PASSWORD, "CHANGE_PASSWORD");
            ValuesDisplay.Add(StatusButton.BACK_CUSTOMER, "BACK_CUSTOMER");

            ValuesDisplay.Add(StatusButton.BACK, "BACK");
            ValuesDisplay.Add(StatusButton.INVOICES, "INVOICES");

            ValuesDisplay.Add(StatusButton.BACK_HOME, "BACK_HOME");
            
            

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

        // GIAO DIEN trang chu
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
                new CardAction  {
                    Title = "Tình trạng đơn hàng",
                    Type = ActionTypes.ImBack,
                    Value = ValuesDisplay.GetValueOrDefault(StatusButton.STATUS_INVOICE),

                }
                

            };

            response.Attachments = new List<Attachment>() { card.ToAttachment() };

            await turnContext.SendActivityAsync(response, cancellationToken);
        }

        private async Task FillOutUserProfileAsync(ConversationFlow flow, UserProfile profile, ITurnContext turnContext, CancellationToken cancellationToken)
        {
            string message;
            string input = turnContext.Activity.Text?.Trim();
            Console.WriteLine(turnContext.Activity.Text);
            if (!profile.IsStartConverstation)  //chua bat dau cuoc tro chuyen
            {

                profile.IsStartConverstation = true;
                await SendIntroCardAsync(turnContext, cancellationToken);

               

            }
            else // trang thai bat dau cuoc tro chuyen
            {
                if (profile.IsStartTransition) // bat dau lien quan van chuyen don han
                {
                    switch (flow.LastDriverQuestion)
                    {

                        case DriverQuestions.NONE:
                            ResetUI(flow, profile);
                            await SendIntroCardAsync(turnContext, cancellationToken);
                            break;

                        case DriverQuestions.USERNAME:
                            // neu bam vao nut back
                            if (input.ToLower().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.BACK_HOME).ToLower()))
                            {
                                profile.IsStartTransition = false;
                                await SendIntroCardAsync(turnContext, cancellationToken);

                            }
                            else
                            {

                                if (ValidateUserName(input, out string username, out message))
                                {

                                    profile.Username = username;
                                    await turnContext.SendActivityAsync($"Bạn vui lòng nhập mật khẩu");
                                    flow.LastDriverQuestion = DriverQuestions.PASSWORD;


                                }
                                else
                                {
                                    
                                    await turnContext.SendActivityAsync(message ?? "Xin lỗi, tôi không hiểu điều bạn đang nói!!");
                                    ResetUI(flow, profile);
                                    await SendIntroCardAsync(turnContext, cancellationToken);
                                }
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
                                profile.IsLogin = true;

                                await turnContext.SendActivityAsync(replyPassword);

                                replyPassword.ChannelData = ButtonInvoice();
                                replyPassword.Text = String.Empty;
                                await turnContext.SendActivityAsync(replyPassword, cancellationToken);
                                Console.WriteLine(turnContext.Activity);
                                flow.LastDriverQuestion = DriverQuestions.INVOICE_CHOOSE;

                            }
                            else
                            {
                                await turnContext.SendActivityAsync(message ?? "Xin lỗi, tôi không hiểu điều bạn đang nói!!");
                                ResetUI(flow, profile);
                                await SendIntroCardAsync(turnContext, cancellationToken);
                            }
                            break;




                        case DriverQuestions.INVOICE_CHOOSE:
                            if (input.ToLower().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.INVOICE_NOW).ToLower()))
                            {
                                flow.LastDriverQuestion = DriverQuestions.INVOICE_NOW;

                                profile.TimeDeliver = DateTime.Now;
                                var customerReadModel = _driverService.GetCustomers(profile.TimeDeliver, profile.Driver.ID);

                                if (!customerReadModel.Any())
                                {
                                    await turnContext.SendActivityAsync("Không có đơn hàng phải trong ngày hôm nay!!!");
                                    ResetUI(flow, profile);
                                    await SendIntroCardAsync(turnContext, cancellationToken);

                                }
                                else
                                {
                                    Console.WriteLine(JsonConvert.SerializeObject(customerReadModel));
                                    Activity activity = turnContext.Activity.CreateReply();
                                    var data = JsonConvert.SerializeObject(WriteReadCustomerFb(customerReadModel, profile));
                                    activity.ChannelData = JsonConvert.DeserializeObject(data);
                                    Console.WriteLine(activity.ChannelData);

                                    await turnContext.SendActivityAsync(activity, cancellationToken);
                                }
                             
                            }
                            else if (input.ToLower().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.INVOICE_PAST).ToLower()))
                            {
                                flow.LastDriverQuestion = DriverQuestions.INVOICE_PAST;
                                await turnContext.SendActivityAsync("Nhập ngày giao (ngày/tháng/năm)");
                                await turnContext.SendActivityAsync("Ví dụ: 23/03/2019");

                            }
                            else if (input.ToLower().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.CHANGE_PASSWORD).ToLower()))
                            {
                                flow.LastDriverQuestion = DriverQuestions.CHANGE_PASSWORD;
                                await turnContext.SendActivityAsync("Nhập mật khẩu mới");

                            }
                            else
                            {
                                ResetUI(flow, profile);
                                await SendIntroCardAsync(turnContext, cancellationToken);
                            }
                          

                            break;
                        //  DANH CHO INVOICE_CHOOSE
                        case DriverQuestions.INVOICE_NOW:
                            string payLoad = SplitCustomerPayLoad(input.Trim(), Key.CUSTOMER, '-');
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
                                    ResetUI(flow, profile);
                                    await SendIntroCardAsync(turnContext, cancellationToken);

                            }
                            break;

                        case DriverQuestions.INVOICE_PAST:
                            if (ValidateTimeDelivery(input, out DateTime? date, out message))
                            {
                                profile.TimeDeliver = date.Value;
                                flow.LastDriverQuestion = DriverQuestions.INVOICE_NOW;
                                var customerReadModel = _driverService.GetCustomers(profile.TimeDeliver, profile.Driver.ID);
                                if(!customerReadModel.Any())
                                {
                                    await turnContext.SendActivityAsync("Không có đơn hàng phải trong ngày !!!");
                                    ResetUI(flow, profile);
                                    await SendIntroCardAsync(turnContext, cancellationToken);

                                }
                                else
                                {
                                     Console.WriteLine(JsonConvert.SerializeObject(customerReadModel));
                                    Activity activity = turnContext.Activity.CreateReply();
                                    var data = JsonConvert.SerializeObject(WriteReadCustomerFb(customerReadModel, profile));
                                    activity.ChannelData = JsonConvert.DeserializeObject(data);
                                    Console.WriteLine(activity.ChannelData);
                                    await turnContext.SendActivityAsync(activity, cancellationToken);
                                }
                           

                               

                            }
                            else
                            {
                                await turnContext.SendActivityAsync(message ?? "Xin lỗi, tôi không hiểu điều bạn đang nói!!");
                                ResetUI(flow, profile);
                                await SendIntroCardAsync(turnContext, cancellationToken);
                            }
                            break;
                        case DriverQuestions.CHANGE_PASSWORD:
                            if(ValidateChangePassword(input, out string newPassword,out message))
                            {
                                var result = _accountService.UpdateAccount(profile.Username, newPassword);
                                if (result)
                                {
                                    await turnContext.SendActivityAsync("Đổi mật khẩu thành công, tài khoản hiện nay là:");
                                    await turnContext.SendActivityAsync($"Tên đăng nhập: {profile.Username}");
                                    await turnContext.SendActivityAsync($"Mật khẩu: {newPassword}");
                                    profile.Password = newPassword;
                                    profile.IsLogin = false;

                                    await turnContext.SendActivityAsync("Bấm ký tự bất kỳ để tiếp tục!!!");
                                    flow.LastDriverQuestion = DriverQuestions.NONE;
                                   
                                }
                            }
                            else
                            {
                                await turnContext.SendActivityAsync(message ?? "Xin lỗi, tôi không hiểu điều bạn đang nói!!");
                                ResetUI(flow, profile);
                                await SendIntroCardAsync(turnContext, cancellationToken);
                            }
                            break;

                        case DriverQuestions.CHOOSE_FUNCTIONAL_CUSTOMER:

                            if (input.Trim().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.INVOICES)))
                            {

                                flow.LastDriverQuestion = DriverQuestions.CHOOSE_STATUS_INVOICE;

                                // reply giao dien quick button reply
                                var replyBack = turnContext.Activity.CreateReply();
                                var invoices = _invoiceService.GetInvoices(profile.TimeDeliver, profile.Driver.ID, profile.Data.CustomerDetail.Customer.Id);
                                profile.Data.Invoices = invoices.ToList();
                                replyBack.ChannelData = ListInvoice(invoices as List<InvoiceReadModel>, profile.Data.CustomerDetail.Customer.Name);
                                await turnContext.SendActivityAsync(replyBack);
                            }
                            else
                            {
                                ResetUI(flow, profile);
                                await SendIntroCardAsync(turnContext, cancellationToken);
                            }

                            break;
                        case DriverQuestions.CHOOSE_STATUS_INVOICE:
                            string key = input.Trim().Split('-').GetValue(0).ToString();
                            Console.Write(key);
                            switch (key)
                            {
                                case "ACCEPT_INVOICE":
                                    string invoiceId = SplitCustomerPayLoad(input.Trim(), Key.ACCEPT_INVOICE, '-');
                                    Console.WriteLine("CHAP NHAN DON HANG");
                                    // cap nhat trang thai don hang
                                    var status = _invoiceService.UpdateVoice(int.Parse(invoiceId, null), (int)StatusInvoice.COMPLETE);
                                    if (status)
                                    {
                                        await turnContext.SendActivityAsync("Cập nhật trạng thái đơn hàng thành công");

                                        profile.Data.Invoices.Find(x => x.Id == int.Parse(invoiceId, null)).Status = (int)StatusInvoice.COMPLETE;
                                        // neu da cap nhat het cac hoa don thi chuyen danh sach khach hang
                                        if (!profile.Data.Invoices.Any(x=>x.Status == (int)StatusInvoice.INPROCESS))
                                        {
                                            _scheduleService.UpdateIsServedRouteInfo(profile.Data.CustomerDetail.Customer.Id, profile.TimeDeliver, profile.Driver.ID, true);
                                             
                                            
                                            // quay ve trang danh sach khach hang
                                            flow.LastDriverQuestion = DriverQuestions.INVOICE_NOW;
                                            var customerReadModel = _driverService.GetCustomers(profile.TimeDeliver, profile.Driver.ID);
                                            Activity activity = turnContext.Activity.CreateReply();
                                            var data = JsonConvert.SerializeObject(WriteReadCustomerFb(customerReadModel, profile));
                                            activity.ChannelData = JsonConvert.DeserializeObject(data);
                                            Console.WriteLine(activity.ChannelData);
                                            await turnContext.SendActivityAsync(activity, cancellationToken);

                                        }
                                        else
                                        {
                                            var replyBack = turnContext.Activity.CreateReply();
                                            var invoices = _invoiceService.GetInvoices(profile.TimeDeliver, profile.Driver.ID, profile.Data.CustomerDetail.Customer.Id);
                                            replyBack.ChannelData = ListInvoice(invoices as List<InvoiceReadModel>, profile.Data.CustomerDetail.Customer.Name);
                                            await turnContext.SendActivityAsync(replyBack);
                                        }
                                      

                                    }
                                    break;
                                case "CANCEL_INVOICE":
                                    invoiceId = SplitCustomerPayLoad(input.Trim(), Key.CANCEL_INVOICE, '-');
                                    status = _invoiceService.UpdateVoice(int.Parse(invoiceId, null), (int)StatusInvoice.CANCEL);
                                    if (status)
                                    {
                                        await turnContext.SendActivityAsync("Cập nhật trạng thái đơn hàng thành công");

                                        profile.Data.Invoices.Find(x => x.Id == int.Parse(invoiceId, null)).Status = (int)StatusInvoice.CANCEL;
                                        // neu da cap nhat het cac hoa don thi chuyen danh sach khach hang
                                        if (profile.Data.Invoices.Find(x => x.Status == (int)StatusInvoice.INPROCESS) is null)
                                        {
                                            _scheduleService.UpdateIsServedRouteInfo(profile.Data.CustomerDetail.Customer.Id, profile.TimeDeliver, profile.Driver.ID, true);
                                            // quay ve trang danh sach khach hang
                                            flow.LastDriverQuestion = DriverQuestions.INVOICE_NOW;
                                            var customerReadModel = _driverService.GetCustomers(profile.TimeDeliver, profile.Driver.ID);
                                            Activity activity = turnContext.Activity.CreateReply();
                                            var data = JsonConvert.SerializeObject(WriteReadCustomerFb(customerReadModel, profile));
                                            activity.ChannelData = JsonConvert.DeserializeObject(data);
                                            Console.WriteLine(activity.ChannelData);
                                            await turnContext.SendActivityAsync(activity, cancellationToken);

                                        }
                                        else
                                        {
                                            var replyBack = turnContext.Activity.CreateReply();
                                            var invoices = _invoiceService.GetInvoices(profile.TimeDeliver, profile.Driver.ID, profile.Data.CustomerDetail.Customer.Id);
                                            replyBack.ChannelData = ListInvoice(invoices as List<InvoiceReadModel>, profile.Data.CustomerDetail.Customer.Name);
                                            await turnContext.SendActivityAsync(replyBack);
                                        }

                                    }
                                    break;
                                default:
                                    break;
                            }

                            if (input.Trim().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.BACK_CUSTOMER)))
                            {
                                // quay ve trang danh sach khach hang
                                flow.LastDriverQuestion = DriverQuestions.INVOICE_NOW;
                                var customerReadModel = _driverService.GetCustomers(profile.TimeDeliver, profile.Driver.ID);
                                Activity activity = turnContext.Activity.CreateReply();
                                var data = JsonConvert.SerializeObject(WriteReadCustomerFb(customerReadModel, profile));
                                activity.ChannelData = JsonConvert.DeserializeObject(data);
                                Console.WriteLine(activity.ChannelData);
                                await turnContext.SendActivityAsync(activity, cancellationToken);
                            }


                                break;
                        default:
                            break;
                    }
                }
                else if (profile.IsStartInvoice) // bat dau trang thai kiem tra tinh trang don hang
                {
                    switch (flow.LastStatusInvoiceQuestion)
                    {
                        case StatusInvoiceQuestion.INPUT_INVOICE_CODE:
                            if(ValadateStatusInvoice(input, out string invoiceCode, out message, out int status))
                            {
                                string messageInvoiceStatus = "";
                                switch (status)
                                {
                                    case (int)StatusInvoice.INPROCESS:
                                        messageInvoiceStatus = "Đang xử lý";
                                        break;
                                    case (int)StatusInvoice.CANCEL:
                                        messageInvoiceStatus = "Đã hủy";
                                        break;
                                    case (int)StatusInvoice.SCHEDULE:
                                        messageInvoiceStatus = "Đã lập lịch";
                                        break;
                                    case (int)StatusInvoice.COMPLETE:
                                        messageInvoiceStatus = "Đã giao";
                                        break;
                                    case (int)StatusInvoice.LOSE_PRODUCT:
                                        messageInvoiceStatus = "Thiếu hàng/cần đổi trả";
                                        break;
                                    default:
                                        break;
                                }
                                await turnContext.SendActivityAsync($"Trạng thái đơn hàng: {messageInvoiceStatus}");
                                await turnContext.SendActivityAsync($"Bấm ký tự bất kỳ để tiếp tục");
                                flow.LastStatusInvoiceQuestion = StatusInvoiceQuestion.NONE;


                            }
                            else
                            {
                                await turnContext.SendActivityAsync(message ?? "Xin lỗi, tôi không hiểu điều bạn đang nói!!");
                                ResetUI(flow, profile);
                                await SendIntroCardAsync(turnContext, cancellationToken);

                            }
                            break;
                        default:
                            ResetUI(flow, profile);
                            await SendIntroCardAsync(turnContext, cancellationToken);
                            break;
                    }

                }
                
                else  // khong nam trong 2 truong hop tren
                {

                    // kiêm tra nguoi dung có bấm nút bd ko
                    if (input.ToLower().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.BeginTransition).ToLower()))
                    {
                        profile.IsStartTransition = true;
                        if (!profile.IsLogin)
                        {
                            await turnContext.SendActivityAsync($"Chào bạn");
                            await turnContext.SendActivityAsync($"Bạn vui lòng nhập tên đăng nhập");
                            flow.LastDriverQuestion = DriverQuestions.USERNAME;
                        }
                        else
                        {
                            flow.LastDriverQuestion = DriverQuestions.INVOICE_CHOOSE;
                            var replyPassword = turnContext.Activity.CreateReply();
                            replyPassword.ChannelData = ButtonInvoice();
                            replyPassword.Text = String.Empty;
                            await turnContext.SendActivityAsync(replyPassword, cancellationToken);
                        }

                        //var reply = turnContext.Activity.CreateReply();
                        //reply.ChannelData = LoginOrChangePassword(profile.IsLogin);
                        //flow.LastDriverQuestion = DriverQuestions.CHOOSE_LOGIN_RESET;

                    }
                    else if (input.ToLower().Equals(ValuesDisplay.GetValueOrDefault(StatusButton.STATUS_INVOICE).ToLower()))
                    {
                        profile.IsStartInvoice = true;
                        await turnContext.SendActivityAsync($"Chào bạn");
                        await turnContext.SendActivityAsync($"Bạn vui lòng nhập mã đơn hàng!!!");
                        flow.LastStatusInvoiceQuestion = StatusInvoiceQuestion.INPUT_INVOICE_CODE;

                    }
                    else
                    {
                        await turnContext.SendActivityAsync($"Xin lỗi bạn, chúng tôi không hiểu ý bạn");
                        await SendIntroCardAsync(turnContext, cancellationToken);
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
        private object WriteReadCustomerFb(IEnumerable<CustomerReadModel> customerReadModels, UserProfile profile)
        {
            List<dynamic> lstInvoice = new List<dynamic>();
            /////
            //List<object> list = new List<object>();
            foreach (var item in customerReadModels)
            {
                
                //string titleSub = $"{item.PhoneNumber}";
                //bool isServed = _driverService.IsServedCustomer(item.Id, profile.TimeDeliver, profile.Driver.ID);
                //if (isServed)
                //{
                //    titleSub = $"{item.PhoneNumber} - Đã giao xong";
                //}
                //list.Add(new
                //{
                //    title = item.Name,
                //    subtitle = titleSub,
                //    buttons = new[]
                //        {
                //            new
                //            {
                //                title = "Xem chi tiết",
                //                type = "postback",
                //                payload = Key.CUSTOMER.ToString() +"-"+item.Id
                //            }
                //        }
                //});

                // add kieu khac
                var lstButton = new List<dynamic>();
                // kiem tra khach hang da duoc giao hay chua
                bool isServed = _driverService.IsServedCustomer(item.Id, profile.TimeDeliver, profile.Driver.ID);
                lstButton.Add(new
                {
                    type = "postback",
                    title = "Xem chi tiết",
                    payload = Key.CUSTOMER.ToString() + "-" + item.Id

                });
                lstButton.Add(new
                {
                    type = "postback",
                    title = "Quay lại",
                    payload = StatusButton.BACK_HOME.ToString()

                });

                if (isServed)
                {
                    lstInvoice.Add(new
                    {
                        title = $"{item.Name} - Đã giao xong",
                        subtitle = $"{item.PhoneNumber}",
                        buttons = lstButton
                    });
                }
                else
                {
                    lstInvoice.Add(new
                    {
                        title = $"{item.Name} - Chưa giao",
                        subtitle = $"{item.PhoneNumber}",
                        buttons = lstButton
                    });
                }
          

            }
            return new
            {
                notification_type = "REGULAR",
                //attachment = new
                //{
                //    type = "template",
                //    payload = new
                //    {
                //        template_type = "list",
                //        top_element_style = "compact",
                //        elements = list,
                //        buttons = new[]
                //        {
                //            new
                //            {
                //                title =  "Quay lại",
                //                type = "postback",
                //                payload = "BACK"
                //            }
                //        }
                //    }
                //}

                attachment = new
                {
                    type = "template",
                    composer_input_disabled = true,
                    payload = new
                    {
                        template_type = "generic",
                        elements = lstInvoice
                    }
                }

            };
        }

        private object LoginOrChangePassword(bool isLogin = false)
        {
            var lstButton = new List<dynamic>();
            if (!isLogin)
            {
                lstButton.Add(new
                {
                    type = "postback",
                    title = $"Đăng nhập",
                    payload = StatusButton.LOGIN.ToString()
                });
            }
            lstButton.Add(new
            {
                type = "postback",
                title = $"Đổi mật khẩU",
                payload = StatusButton.CHANGE_PASSWORD.ToString()
            });


            return new
            {
                attachment = new
                {
                    type = "template",
                    payload = new
                    {
                        template_type = "button",
                        text = "Chọn chức năng?",
                        buttons = lstButton
                    }
                }
            };
        }

        private object ButtonInvoice()
        {

            return new
            {
                attachment = new
                {
                    type = "template",
                    payload = new
                    {
                        template_type = "button",
                        text = "Chọn chức năng?",
                        buttons = new List<dynamic>
                        {
                            new
                            {
                                type = "postback",
                                title = $"Đơn hàng hôm nay",
                                payload = "INVOICE_NOW"
                            },
                             new
                            {
                                type = "postback",
                                title = $"Đơn hàng khác",
                                payload = "INVOICE_PAST"
                            },
                             new
                            {
                                type = "postback",
                                title = $"Đổi mật khẩu",
                                payload = StatusButton.CHANGE_PASSWORD.ToString()
                            },

                        }
                    }
                }
            };
        }

        private object Menu()
        {
            return new
            {
                
                persistent_menu = new List<dynamic>
                {
                    new
                    {
                         locale = "vi_VI",
                         composer_input_disabled = false,
                         call_to_actions = new List<dynamic>
                         {
                             new
                             {
                                  title = "My Account",
                                  type = "postback",
                                  payload = "PAYBILL_PAYLOAD"
                             },
                              new
                             {
                                  title = "Contact Me",
                                  type = "postback",
                                  payload = "PAYBILL_PAYLOAD"
                             },
                         },
                    },
                       new
                    {
                         locale = "vi_VI",
                         composer_input_disabled = false,
                    },
                }
            };
        }

        private object BackHome()
        {
            return new
            {
                attachment = new
                {
                    type = "template",
                    payload = new
                    {
                        template_type = "button",
                        text = "Bạn muốn?",
                        buttons = new List<dynamic>
                        {
                            new
                            {
                                title = "Trang chủ",
                                type = "postback",
                                payload = StatusButton.BACK_HOME.ToString(),
                            },
                            new
                            {
                                title = "Nhập lại",
                                type = "postback",
                                 payload = StatusButton.BACK_HOME.ToString(),
                            },


                        }
                    }
                }
            };
        }

        // Ve giao dien danh sach cac don hang
        private object ListInvoice(List<InvoiceReadModel> invoiceReads, string customerName)
        {
            List<dynamic> lstInvoice = new List<dynamic>();
            foreach (var invoice in invoiceReads)
            {
                var lstButton = new List<dynamic>();
                if (invoice.DeliveryTime.Date >= DateTime.Now.Date)
                {
                    if ((int)StatusInvoice.CANCEL == invoice.Status)
                    {
                        lstButton.Add(new
                        {
                            type = "postback",
                            title = "Đã giao đơn hàng",
                            payload = $"{Key.ACCEPT_INVOICE}-{invoice.Id}"

                        });

                    }
                    else if ((int)StatusInvoice.COMPLETE == invoice.Status)
                    {
                        lstButton.Add(new
                        {
                            type = "postback",
                            title = "Hủy đơn hàng",
                            payload = $"{Key.CANCEL_INVOICE}-{invoice.Id}"

                        });

                    }
                    else if ((int)StatusInvoice.INPROCESS == invoice.Status || (int)StatusInvoice.SCHEDULE == invoice.Status)
                    {
                        lstButton.Add(new
                        {
                            type = "postback",
                            title = "Xác nhận giao hàng",
                            payload = $"{Key.ACCEPT_INVOICE}-{invoice.Id}"

                        });
                        lstButton.Add(new
                        {
                            type = "postback",
                            title = "Hủy đơn hàng",
                            payload = $"{Key.CANCEL_INVOICE}-{invoice.Id}"

                        });
                    }
                }
                
               

                lstButton.Add(new
                {
                    type = "postback",
                    title = "Quay lại",
                    payload = StatusButton.BACK_CUSTOMER.ToString()

                });


                lstInvoice.Add(new
                {
                    title = $"{customerName} - {invoice.Code}",
                    subtitle = $"Trạng thái: {ConvertStatusInvoice(invoice.Status)} ",
                    buttons = lstButton
                });
            }
            return new
            {
                attachment = new
                {
                    type = "template",
                    composer_input_disabled = true,
                    payload = new
                    {
                        template_type = "generic",
                        elements = lstInvoice
                    }
                }
            };
        }


        // ve giao dien quick reply
        private object QuickReply()
        {
            return new
            {
                text = "Bạn muốn?",
                quick_replies = new[]
                {
                    new
                    {
                        content_type = "text",
                        title = "Trang chủ",
                        payload = StatusButton.BACK_HOME.ToString(),
                    },
                     new
                    {
                        content_type = "text",
                        title = "Nhập lại",
                        payload = "po",
                    }
                }
            };
        }

        private object DetailCustomer(CustomerDetailReadModel customerDetail)
        {
            Console.WriteLine(customerDetail.Customer.PhoneNumber.ToString());
            Console.WriteLine(customerDetail.Address.Lat + "," + customerDetail.Address.Lng);
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
                        string.Format("{0}-{1}-{2}-{3}", customerDetail.Address.StreetNumber,
                            customerDetail.Address.Street, customerDetail.Address.District, customerDetail.Address.City),
                        buttons = new List<dynamic>
                        {
                            new
                            {
                                title = "Đơn hàng cần chuyển",
                                type = "postback",
                                payload = StatusButton.INVOICES.ToString(),
                            },
                            new
                            {
                                title = "Xem chỉ đường",
                                type = "web_url",
                                url = string.Format("https://www.google.com/maps/dir/?api=1&destination={0},{1}", customerDetail.Address.Lat, customerDetail.Address.Lng)
                            },
                              new
                            {
                                title = "Gọi điện",
                                type = "phone_number",
                                payload = string.Format("+84{0}",customerDetail.Customer.PhoneNumber.Substring(1)),

                            }


                        }
                    }
                }
            };
        }

        private string ConvertStatusInvoice(int status)
        {
            switch (status)
            {
                case 0:
                    return "Đang xử lí";
                case 1:
                    return "Thiếu hàng/ cần đổi trả";
                case 2:
                    return "Đã giao hàng";
                case 3:
                    return "Hủy giao hàng";
                case 4:
                    return "Đã xếp lịch";
                default:
                    return "";
            }
        }

        #endregion

        /// <summary>
        /// lay ra 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string SplitCustomerPayLoad(string payload, Key key, char split)
        {
            var isCorrect = payload.Contains(key.ToString());
            return isCorrect ? payload.Split(split).GetValue(1).ToString() : string.Empty;
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
                message = isExisit ? null : "Tài khoản không tồn tại.";
            }

            return message is null;
        }

        private bool ValadateStatusInvoice(string input, out string invoiceCode, out string message, out int status)
        {
            message = null;
            invoiceCode = null;
            status = 0;
            if (string.IsNullOrWhiteSpace(input))
            {
                message = "Mã code không được để trống";
            }
            else
            {
                invoiceCode = input.Trim();
                var invoice = _invoiceService.GetInvoice(invoiceCode);
                if(invoice == null)
                {
                    message = $"Không tồn tại đơn hàng với mã là: {invoiceCode} ";
                }
                else
                {
                    status = invoice.Status;
                }
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
        private bool ValidateChangePassword(string input, out string newPassword, out string message)
        {
            newPassword = null;
            message = null;
            
            if (string.IsNullOrWhiteSpace(input))
            {
                message = "Hãy nhập ít nhất 1 ký tự.";
            }
            else
            {
                newPassword = input;

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

        private static bool ValidateTimeDelivery(string input, out DateTime? date, out string message)
        {
            message = null;
            date = null;
            string dateDelivery = input.Trim();
            bool check = DateTime.TryParseExact(dateDelivery, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dateResult);
            if (check)
            {
                //if (dateResult.Date >= DateTime.Now.AddDays(1).Date)
                //{
                //    message = "Ngày giao hàng không lớn hơn ngày hiện tại";

                //}
                date = dateResult;
            }
            else
            {
                message = "Định dạng chưa hợp lệ !!!";
            }
            return message is null;

        }

        private void ResetUI(ConversationFlow flow, UserProfile profile)
        {
            flow.LastDriverQuestion = DriverQuestions.NONE;
            flow.LastStatusInvoiceQuestion = StatusInvoiceQuestion.NONE;
            profile.IsStartInvoice = false;
            profile.IsStartTransition = false;

        }

        public enum StatusButton
        {
            BeginTransition,
            STATUS_INVOICE,
            CHANGE_PASSWORD,

            // vs BeginTransition
            INVOICE_NOW,
            INVOICE_PAST,
            BACK,
            ADDRESS,
            INVOICES,

            LOGIN,
            BACK_CUSTOMER,

            // Voi STATUS_INVOICE

            BACK_HOME

        }
        public enum Key
        {
            CUSTOMER,
            ACCEPT_INVOICE,
            CANCEL_INVOICE
        }

        public enum StatusInvoice
        {
            INPROCESS = 0,
            LOSE_PRODUCT = 1,
            COMPLETE = 2,
            CANCEL = 3,
            SCHEDULE = 4
        }

    }

}
