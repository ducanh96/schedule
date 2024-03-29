﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Linq;
using EchoBot.DialogPrompt;
using EchoBot.DialogPrompt.Model;
using EchoBot.Model;
using EchoBot.PromptUsers;
using EchoBot.WelcomeUser;
using EchoBot.WelcomeUser.Model;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Integration;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TransitionApp.Domain.Bus;
using TransitionApp.Domain.Interface.Repository;
using TransitionApp.Infrastructor.Implement.Repository;
using TransitionApp.Service.Implement;
using TransitionApp.Service.Interface;

namespace EchoBot
{
    /// <summary>
    /// The Startup class configures services and the request pipeline.
    /// </summary>
    public class Startup
    {
        private ILoggerFactory _loggerFactory;
        private readonly bool _isProduction;

        public Startup(IHostingEnvironment env)
        {
            _isProduction = env.IsProduction();
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration that represents a set of key/value application configuration properties.
        /// </summary>
        /// <value>
        /// The <see cref="IConfiguration"/> that represents a set of key/value application configuration properties.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> specifies the contract for a collection of service descriptors.</param>
        /// <seealso cref="IStatePropertyAccessor{T}"/>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/dependency-injection"/>
        /// <seealso cref="https://docs.microsoft.com/en-us/azure/bot-service/bot-service-manage-channels?view=azure-bot-service-4.0"/>
        public void ConfigureServices(IServiceCollection services)
        {
            var secretKey = Configuration.GetSection("botFileSecret")?.Value;
            var botFilePath = Configuration.GetSection("botFilePath")?.Value;
            if (!File.Exists(botFilePath))
            {
                throw new FileNotFoundException($"The .bot configuration file was not found. botFilePath: {botFilePath}");
            }

            // Loads .bot configuration file and adds a singleton that your Bot can access through dependency injection.
            BotConfiguration botConfig = null;
            try
            {
                botConfig = BotConfiguration.Load(botFilePath, secretKey);
            }
            catch
            {
                var msg = @"Error reading bot file. Please ensure you have valid botFilePath and botFileSecret set for your environment.
        - You can find the botFilePath and botFileSecret in the Azure App Service application settings.
        - If you are running this bot locally, consider adding a appsettings.json file with botFilePath and botFileSecret.
        - See https://aka.ms/about-bot-file to learn more about .bot file its use and bot configuration.
        ";
                throw new InvalidOperationException(msg);
            }

            services.AddSingleton(sp => botConfig ?? throw new InvalidOperationException($"The .bot configuration file could not be loaded. botFilePath: {botFilePath}"));

            // Retrieve current endpoint.
            var environment = _isProduction ? "production" : "development";
            var service = botConfig.Services.FirstOrDefault(s => s.Type == "endpoint" && s.Name == environment);
            if (service == null && _isProduction)
            {
                // Attempt to load development environment
                service = botConfig.Services.Where(s => s.Type == "endpoint" && s.Name == "development").FirstOrDefault();
            }

            if (!(service is EndpointService endpointService))
            {
                throw new InvalidOperationException($"The .bot file does not contain an endpoint with name '{environment}'.");
            }

            // Memory Storage is for local bot debugging only. When the bot
            // is restarted, everything stored in memory will be gone.
            IStorage dataStore = new MemoryStorage();

            // For production bots use the Azure Blob or
            // Azure CosmosDB storage providers. For the Azure
            // based storage providers, add the Microsoft.Bot.Builder.Azure
            // Nuget package to your solution. That package is found at:
            // https://www.nuget.org/packages/Microsoft.Bot.Builder.Azure/
            // Un-comment the following lines to use Azure Blob Storage
            // // Storage configuration name or ID from the .bot file.
            // const string StorageConfigurationId = "<STORAGE-NAME-OR-ID-FROM-BOT-FILE>";
            // var blobConfig = botConfig.FindServiceByNameOrId(StorageConfigurationId);
            // if (!(blobConfig is BlobStorageService blobStorageConfig))
            // {
            //    throw new InvalidOperationException($"The .bot file does not contain an blob storage with name '{StorageConfigurationId}'.");
            // }
            // // Default container name.
            // const string DefaultBotContainer = "<DEFAULT-CONTAINER>";
            // var storageContainer = string.IsNullOrWhiteSpace(blobStorageConfig.Container) ? DefaultBotContainer : blobStorageConfig.Container;
            // IStorage dataStore = new Microsoft.Bot.Builder.Azure.AzureBlobStorage(blobStorageConfig.ConnectionString, storageContainer);

            // Create and add conversation state.
            var conversationState = new ConversationState(dataStore);
            services.AddSingleton(conversationState);

            #region Customize

            UserState userState = new UserState(dataStore);
            services.AddSingleton<EchoBotAccessors>(sp =>
            {
                return new EchoBotAccessors(conversationState, userState)
                {
                    ConversationDataAccessor = conversationState.CreateProperty<ConversationData>(EchoBotAccessors.ConversationDataName),
                    UserProfileAccessor = userState.CreateProperty<UserProfile>(EchoBotAccessors.UserProfileName),
                };
            });

           
           

            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddTransient<IDriverService, DriverService>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IScheduleService, ScheduleService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IScheduleRepository, ScheduleRepository>();
            #region Prompt for dialog

            //// Create and register state accesssors.
            //services.AddSingleton<DialogPromptBotAccessors>(sp =>
            //{
            //    // State accessors enable other components to read and write individual properties of state.
            //    var accessors = new DialogPromptBotAccessors(conversationState)
            //    {
            //        DialogStateAccessor =
            //            conversationState.CreateProperty<DialogState>(
            //                DialogPromptBotAccessors.DialogStateAccessorKey),
            //        ReservationAccessor =
            //            conversationState.CreateProperty<Reservation>(
            //                DialogPromptBotAccessors.ReservationAccessorKey),
            //    };

            //    return accessors;
            //});


            #endregion

            #region Prompt For User

            // Create and register state accessors.
            // Accessors created here are passed into the IBot-derived class on every turn.
            services.AddSingleton<CustomPromptBotAccessors>(sp =>
            {
                // Create the custom state accessor.
                return new CustomPromptBotAccessors(conversationState, userState)
                {
                    ConversationFlowAccessor = conversationState.CreateProperty<ConversationFlow>(CustomPromptBotAccessors.ConversationFlowName),
                    UserProfileAccessor = userState.CreateProperty<UserProfile>(CustomPromptBotAccessors.UserProfileName),
                };
            });


            services.AddSingleton<WelcomeUserStateAccessors>(sp =>
            {
                // Create the custom state accessor.
                return new WelcomeUserStateAccessors(userState, conversationState)
                {
                    WelcomeUserState = userState.CreateProperty<WelcomeUserState>(WelcomeUserStateAccessors.WelcomeUserName),

                   ConversationDriverFlowAccessor = conversationState.CreateProperty<ConversationDriverFlow>(WelcomeUserStateAccessors.ConversationDriverFlowName)
                };
            });

            services.AddBot<CustomPromptBot>(options =>
            {
                options.CredentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);

                // Creates a logger for the application to use.
                ILogger logger = _loggerFactory.CreateLogger<CustomPromptBot>();

                // Catches any errors that occur during a conversation turn and logs them.
                options.OnTurnError = async (context, exception) =>
                {
                    logger.LogError($"Exception caught : {exception}");
                    await context.SendActivityAsync("Sorry, it looks like something went wrong.");
                };
            });

            //services.AddBot<WelcomeUserBot>(options =>
            //{
            //    options.CredentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);

            //    // Creates a logger for the application to use.
            //    ILogger logger = _loggerFactory.CreateLogger<WelcomeUserBot>();

            //    // Catches any errors that occur during a conversation turn and logs them.
            //    options.OnTurnError = async (context, exception) =>
            //    {
            //        logger.LogError($"Exception caught : {exception}");
            //        await context.SendActivityAsync("Sorry, it looks like something went wrong.");
            //    };
            //});



            #endregion


            #endregion


            // services.AddBot<EchoBotBot>(options =>
            //{
            //    options.CredentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);

            //     // Catches any errors that occur during a conversation turn and logs them to currently
            //     // configured ILogger.
            //     ILogger logger = _loggerFactory.CreateLogger<EchoBotBot>();

            //    options.OnTurnError = async (context, exception) =>
            //    {
            //        logger.LogError($"Exception caught : {exception}");
            //        await context.SendActivityAsync("Sorry, it looks like something went wrong.");
            //    };
            //});




        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseBotFramework();
        }
    }
}
